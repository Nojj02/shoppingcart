using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Web
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
    }
}