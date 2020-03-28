using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ShoppingCartHandlers.Web
{
    public class EventApi : IEventApi
    {
        private readonly string _host;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly EventConverter _eventConverter;
        private readonly int _batchSize;

        public EventApi(
            string host,
            IHttpClientWrapper httpClientWrapper,
            EventConverter eventConverter,
            int batchSize)
        {
            _host = host;
            _httpClientWrapper = httpClientWrapper;
            _eventConverter = eventConverter;
            _batchSize = batchSize;
        }

        public async Task<IList<object>> GetEventsAfterAsync(string resourceName, MessageNumber lastMessageNumber)
        {
            var allEvents = new List<object>();

            List<object> retrievedEventsList;
            MessageNumberRange numberRange;
            if (lastMessageNumber == MessageNumber.NotSet)
            {
                numberRange = new MessageNumberRange(MessageNumber.New(0), _batchSize);

                retrievedEventsList = await GetAsync(resourceName, numberRange);
                allEvents.AddRange(retrievedEventsList);
            }
            else
            {
                var skippedBatches = lastMessageNumber.Value / _batchSize;
                numberRange = new MessageNumberRange(MessageNumber.New(skippedBatches * _batchSize), _batchSize);

                retrievedEventsList = await GetAsync(resourceName, numberRange);
                var skippedBatchSize = lastMessageNumber.Value % _batchSize;
                var eventsListStartingFromLastMessageNumber = retrievedEventsList.Skip(skippedBatchSize + 1).ToList();

                allEvents.AddRange(eventsListStartingFromLastMessageNumber);
            }

            while (retrievedEventsList.Count == _batchSize)
            {
                numberRange = new MessageNumberRange(MessageNumber.New(numberRange.End.Value + 1), _batchSize);
                retrievedEventsList = await GetAsync(resourceName, numberRange);
                allEvents.AddRange(retrievedEventsList);
            }

            return allEvents;
        }

        private async Task<List<object>> GetAsync(string resourceName, MessageNumberRange range)
        {
            var url = new Uri(new Uri(_host), relativeUri: Path.Combine("events", resourceName, $"{range.Start}-{range.End}"));
            Console.WriteLine($"Getting events from {url}");
            var message =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: url);

            var responseMessage = await _httpClientWrapper.SendAsync(message);
            if (!responseMessage.IsSuccessStatusCode) throw new Exception();

            if (!responseMessage.IsSuccessStatusCode) throw new EventApiRequestFailedException(url, statusCode: responseMessage.StatusCode);

            if (responseMessage.Content == null) return new List<object>();

            var content = await responseMessage.Content.ReadAsStringAsync();

            return MapJsonContentToObjects(content);
        }

        private List<object> MapJsonContentToObjects(string content)
        {
            if (content == String.Empty) return new List<object>();

            var json = JObject.Parse(content);

            var eventsJson = json["events"].Select(x =>
            {
                var type = _eventConverter.GetTypeOf((string)x["eventType"]);
                return x["event"].ToObject(type);
            }).ToList();
            return eventsJson;
        }

        private class MessageNumberRange
        {
            public MessageNumberRange(MessageNumber start, int batchSize)
            {
                if (start == MessageNumber.NotSet)
                {
                    throw new InvalidMessageNumberRangeException("Range cannot start with NotSet value");
                }

                Start = start;
                End = MessageNumber.New(start.Value + batchSize - 1);
            }
            
            public MessageNumber Start { get; }

            public MessageNumber End { get; }
        }

        public class InvalidMessageNumberRangeException : Exception
        {
            public InvalidMessageNumberRangeException(string message)
                : base(message)
            {

            }
        }
    }

    public class EventApiRequestFailedException : Exception
    {
        private readonly Uri _url;
        private readonly HttpStatusCode _statusCode;

        public EventApiRequestFailedException(Uri url, HttpStatusCode statusCode)
        {
            _url = url;
            _statusCode = statusCode;
        }

        public override string Message => $"Event API request failed [{Enum.GetName(typeof(HttpStatusCode), _statusCode)}] to {_url}";
    }
}
