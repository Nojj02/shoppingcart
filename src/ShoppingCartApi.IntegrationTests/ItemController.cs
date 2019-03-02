using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests
{
    public class ComputeCostTests
    {
        [Fact]
        public async Task CreateNewItem()
        {
            await GivenAShopWithItems(
                new List<dynamic>
                {
                    new {Code = "potato", Price = 30},
                    new {Code = "apple", Price = 70},
                    new {Code = "tomato", Price = 50},
                });

            await UserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 160);
        }

        private async Task UserCanComputeTotalCostOfShoppingItems(
            List<dynamic> shoppingItems,
            decimal expectedTotalCost)
        {
            var computeCostDto =
                new
                {
                    ShoppingItems =
                        shoppingItems
                            .Select(x =>
                                new
                                {
                                    ItemCode = x.ItemCode,
                                    Quantity = x.Quantity
                                })
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(computeCostDto));
            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri("http://localhost:9050/calculator/computeCost"))
                {
                    Content = httpContent
                };
            
            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);
                
                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);

                var body = 
                    JsonConvert.DeserializeAnonymousType(await postResponse.Content.ReadAsStringAsync(),
                        new
                        {
                            TotalCost = 0.0m
                        });
                
                Assert.Equal(expectedTotalCost, body.TotalCost);
            }
        }

        private async Task GivenAShopWithItems(List<dynamic> items)
        {
            foreach (var item in items)
            {
                var itemDto = new
                {
                    item.Code
                };

                var httpContent = new StringContent(JsonConvert.SerializeObject(itemDto));

                var postRequestMessage =
                    new HttpRequestMessage(
                        method: HttpMethod.Post,
                        requestUri: new Uri("http://localhost:9050/items"))
                    {
                        Content = httpContent
                    };
                
                using (var httpClient = new HttpClient())
                {
                    var postResponse = await httpClient.SendAsync(postRequestMessage);

                    Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
                }
            }
        }
    }
}