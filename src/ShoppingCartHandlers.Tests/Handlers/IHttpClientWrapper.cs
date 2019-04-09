using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}