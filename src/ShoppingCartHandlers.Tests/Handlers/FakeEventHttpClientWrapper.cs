using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class FakeEventHttpClientWrapper : IHttpClientWrapper
    {
        private readonly List<HttpRequestMessage> _messagesSent = new List<HttpRequestMessage>();

        private readonly string _message;

        public FakeEventHttpClientWrapper()
        {
        }

        public FakeEventHttpClientWrapper(string messageType, IEnumerable<object> events)
        {
            var message = new
            {
                messageType = messageType,
                events = events
            };
            _message = JsonConvert.SerializeObject(message);
        }

        public IReadOnlyList<HttpRequestMessage> MessagesSent => _messagesSent;

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            _messagesSent.Add(httpRequestMessage);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (_message != null)
            {
                response.Content = new StringContent(_message);
            }

            return Task.FromResult(response);
        }
    }
}