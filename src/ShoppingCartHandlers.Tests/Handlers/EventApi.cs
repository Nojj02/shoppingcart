using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventApi : IEventApi
    {
        private readonly string _host;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly EventConverter _eventConverter;
        private readonly int _batchSize;
        private readonly IEventTrackingRepository _eventTrackingRepository;

        public EventApi(
            string host, 
            IHttpClientWrapper httpClientWrapper, 
            EventConverter eventConverter, 
            int batchSize,
            IEventTrackingRepository eventTrackingRepository)
        {
            _host = host;
            _httpClientWrapper = httpClientWrapper;
            _eventConverter = eventConverter;
            _batchSize = batchSize;
            _eventTrackingRepository = eventTrackingRepository;
        }

        public async Task<IList<object>> GetNewEventsAsync(string resourceName)
        {
            var lastMessageNumber = _eventTrackingRepository.GetLastMessageNumber(resourceName);

            var batchesSkipped = lastMessageNumber / _batchSize;
            var startRange = batchesSkipped * _batchSize;
            var endRange = startRange + _batchSize - 1;
            
            var url = new Uri(Path.Combine(_host, resourceName, $"{startRange}-{endRange}"));
            var message =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: url);

            var responseMessage = await _httpClientWrapper.SendAsync(message);

            if (responseMessage.Content == null) return new List<object>();

            var content = await responseMessage.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);

            var eventsJson = json["events"].Select(x =>
            {
                var type = _eventConverter.GetTypeOf((string)x["eventType"]);
                return x["event"].ToObject(type);
            }).ToList();

            return eventsJson;
        }
    }
}
