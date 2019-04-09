using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class FakeHttpClientWrapper : IHttpClientWrapper
    {
        private readonly string _message;

        public FakeHttpClientWrapper(string message)
        {
            _message = message;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }
}