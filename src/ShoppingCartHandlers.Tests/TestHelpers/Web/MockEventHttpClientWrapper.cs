using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers.Tests.TestHelpers.Web
{
    public class MockEventHttpClientWrapper : IHttpClientWrapper
    {
        private readonly string _content;
        private readonly List<HttpRequestMessage> _messagesSent = new List<HttpRequestMessage>();

        public MockEventHttpClientWrapper(string content)
        {
            _content = content;
        }

        public IReadOnlyList<HttpRequestMessage> MessagesSent => _messagesSent;
        
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            _messagesSent.Add(httpRequestMessage);
            
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = _content != null ? new StringContent(_content) : null
            });
        }
    }
}