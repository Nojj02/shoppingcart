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

        public EventApi(string host, IHttpClientWrapper httpClientWrapper, EventConverter eventConverter)
        {
            _host = host;
            _httpClientWrapper = httpClientWrapper;
            _eventConverter = eventConverter;
        }

        public async Task<IList<object>> GetNewEventsAsync(string resourceName)
        {
            var url = new Uri(Path.Combine(_host, resourceName, "0-10"));
            var message =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: url);

            var responseMessage = await _httpClientWrapper.SendAsync(message);

            if (responseMessage.Content == null) return new List<object>();

            var content = await responseMessage.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);

            var type = _eventConverter.GetTypeOf((string)json["messageType"]);

            var eventsJson = json["events"].Select(x => x.ToObject(type)).ToList();

            return eventsJson;
        }
    }
}
