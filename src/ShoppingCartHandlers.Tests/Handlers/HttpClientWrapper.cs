using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.SendAsync(httpRequestMessage);
            }
        }
    }
}