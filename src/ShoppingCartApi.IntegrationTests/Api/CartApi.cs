using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests.Api
{
    public class CartApi
    {
        private readonly ApiUrl _apiUrl;

        public CartApi(ApiUrl apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<CartDto> GetAsync(Guid id)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri(_apiUrl.GetFor($"/cart/{id}")));

            using var httpClient = new HttpClient();

            var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

            Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

            return JsonConvert.DeserializeObject<CartDto>(await getItemResponse.Content.ReadAsStringAsync());
        }

        public class CartDto
        {
            public Guid Id { get; set; }

            public decimal TotalCost { get; set; }
        }
    }
}