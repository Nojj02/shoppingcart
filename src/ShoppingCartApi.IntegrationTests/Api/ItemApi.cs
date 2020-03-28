using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests.Api
{
    public class ItemApi
    {
        private readonly ApiUrl _apiUrl;

        public ItemApi(ApiUrl apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<ItemDto> GetByCodeAsync(string code)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri(_apiUrl.GetFor($"/items?code={code}")));

            using var httpClient = new HttpClient();

            var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

            Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

            return JsonConvert.DeserializeObject<ItemDto>(await getItemResponse.Content.ReadAsStringAsync());
        }

        public class ItemDto
        {
            public Guid Id { get; set; }
        }
    }
}