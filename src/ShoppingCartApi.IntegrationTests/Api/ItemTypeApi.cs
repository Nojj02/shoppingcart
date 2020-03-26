using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests.Api
{
    public class ItemTypeApi
    {
        private readonly ApiUrl _apiUrl;

        public ItemTypeApi(ApiUrl apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<ItemTypeDto> GetByCodeAsync(string code)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri(_apiUrl.GetFor($"/itemTypes?code={code}")));

            using var httpClient = new HttpClient();
            var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

            if (getItemResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

            return JsonConvert.DeserializeObject<ItemTypeDto>(await getItemResponse.Content.ReadAsStringAsync());
        }

        public class ItemTypeDto
        {
            public Guid Id { get; set; }
        }
    }
}