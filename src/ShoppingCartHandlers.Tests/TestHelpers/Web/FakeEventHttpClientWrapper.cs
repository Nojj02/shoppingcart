using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers.Tests.TestHelpers.Web
{
    public class FakeEventHttpClientWrapper : IHttpClientWrapper
    {
        private readonly string _resourceName;
        private readonly IEnumerable<EventInfo> _events = new List<EventInfo>();
        private readonly List<HttpRequestMessage> _messagesSent = new List<HttpRequestMessage>();

        public FakeEventHttpClientWrapper()
        {
        }

        public FakeEventHttpClientWrapper(string resourceName, IEnumerable<EventInfo> events)
        {
            _resourceName = resourceName;
            _events = events;
        }

        public IReadOnlyList<HttpRequestMessage> MessagesSent => _messagesSent;

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            _messagesSent.Add(httpRequestMessage);
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            if (!_events.Any()) return Task.FromResult(response);

            var expectedIndexSegment = httpRequestMessage.RequestUri.AbsolutePath.Split('/').LastOrDefault();
            if (expectedIndexSegment == null) return Task.FromResult(response);

            var rangeIndices = 
                expectedIndexSegment
                    .Split('-')
                    .Select(x => Convert.ToInt32(x))
                    .ToList();

            if (rangeIndices.Count < 2) return Task.FromResult(response);

            var start = rangeIndices[0];
            var end = rangeIndices[1];
            var batchSize = end - start + 1;

            var events = _events.Skip(start).Take(batchSize).ToList();

            var serializedMessage = SerializeEvents(_resourceName, events);
            response.Content = new StringContent(serializedMessage);

            return Task.FromResult(response);
        }

        private static string SerializeEvents(string resourceName, IEnumerable<EventInfo> events)
        {
            var message = new
            {
                messageType = resourceName,
                events = events
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializedMessage = JsonConvert.SerializeObject(message, serializerSettings);
            return serializedMessage;
        }
    }
}