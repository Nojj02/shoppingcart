using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class FakeHttpClientWrapper : IHttpClientWrapper
    {
        private readonly List<HttpRequestMessage> _messagesSent = new List<HttpRequestMessage>();

        private readonly string _message;

        public FakeHttpClientWrapper(string message = null)
        {
            _message = message;
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