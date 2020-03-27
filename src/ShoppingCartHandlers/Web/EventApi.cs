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

        public async Task<IList<object>> GetAllEventsAsync(string resourceName)
        {
            return await GetEventsAfterAsync(resourceName, -1);
        }

        public async Task<IList<object>> GetEventsAfterAsync(string resourceName, int lastMessageNumber)
        {
            var allEvents = new List<object>();

            var skippedBatches = lastMessageNumber / _batchSize;
            var start = skippedBatches * _batchSize;
            var end = start + _batchSize - 1;
            
            var retrievedEventsList = await GetAsync(resourceName, start, end);
            var skippedInCurrentBatch = lastMessageNumber % _batchSize;
            var eventsListStartingFromLastMessageNumber = retrievedEventsList.Skip(skippedInCurrentBatch).ToList();
            
            allEvents.AddRange(eventsListStartingFromLastMessageNumber);

            while (retrievedEventsList.Count == _batchSize)
            {
                start = end + 1;
                end = start + _batchSize - 1;
                retrievedEventsList = await GetAsync(resourceName, start, end);
                allEvents.AddRange(retrievedEventsList);
            }

            return allEvents;
        }

        private async Task<List<object>> GetAsync(string resourceName, int start, int end)
        {
            var url = new Uri(new Uri(_host), relativeUri: Path.Combine(resourceName, $"{start}-{end}"));
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
