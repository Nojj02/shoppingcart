using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartApi.IntegrationTests
{
    public static class Steps
    {
        public static async Task GivenAShopWithItems(List<dynamic> items)
        {
            foreach (var item in items)
            {
                var itemDto = new
                {
                    Code = item.Code,
                    Price = item.Price
                };

                var httpContent = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");

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

        public static async Task UserCanComputeTotalCostOfShoppingItems(
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

            var httpContent = new StringContent(JsonConvert.SerializeObject(computeCostDto), Encoding.UTF8, "application/json");
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

        public static async Task GivenItemIsDiscounted(string itemCode, int percentOff)
        {
            var setAsDiscountedDto =
                new
                {
                    PercentOff = percentOff
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(setAsDiscountedDto), Encoding.UTF8, "application/json");
            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri($"http://localhost:9050/item/{itemCode}/setAsDiscounted"))
                {
                    Content = httpContent
                };

            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);

                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            }
        }
    }
}