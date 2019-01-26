using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests
{
    public class ItemController
    {
        [Fact]
        public async Task CreateNewItem()
        {
            using (var httpClient = new HttpClient())
            {
                var item = new
                {
                    Code = "potato"
                };
                var httpContent = new StringContent(JsonConvert.SerializeObject(item));

                var postRequestMessage =
                    new HttpRequestMessage(
                        method: HttpMethod.Post,
                        requestUri: new Uri("http://localhost:9050/items"))
                    {
                        Content = httpContent
                    };

                var postResult = await httpClient.SendAsync(postRequestMessage);
                
                Assert.Equal(HttpStatusCode.Created, postResult.StatusCode);

                var postResultObject = JsonConvert.DeserializeAnonymousType(await postResult.Content.ReadAsStringAsync(),
                    new
                    {
                        Code = ""
                    });

                var getRequestMessage =
                    new HttpRequestMessage(
                        method: HttpMethod.Get,
                        requestUri: new Uri($"http://localhost:9050/items/{postResultObject.Code}"))
                    {
                        Content = httpContent
                    };

                var getResult = await httpClient.SendAsync(getRequestMessage);
                Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);
            }
        }
    }
}