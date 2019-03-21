using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests.Api
{
    public static class ItemApi
    {
        private const string Host = "localhost:9050";

        public static async Task<ItemDto> GetByItemCodeAsync(string itemCode)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri($"http://{Host}/items?code={itemCode}"));

            using (var httpClient = new HttpClient())
            {
                var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

                Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

                return JsonConvert.DeserializeObject<ItemDto>(await getItemResponse.Content.ReadAsStringAsync());
            }
        }

        public class ItemDto
        {
            public Guid Id { get; set; }
        }
    }
}