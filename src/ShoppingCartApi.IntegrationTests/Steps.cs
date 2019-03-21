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

        public static async Task GivenItemIsDiscountedByAPercentage(string itemCode, int percentOff)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri($"http://localhost:9050/items?code={itemCode}"));

            Guid itemId;

            using (var httpClient = new HttpClient())
            {
                var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

                Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);
                
                var body =
                    JsonConvert.DeserializeAnonymousType(await getItemResponse.Content.ReadAsStringAsync(),
                        new
                        {
                            Id = Guid.Empty
                        });

                itemId = body.Id;
            }

            var setDiscountDto =
                new
                {
                    PercentOff = percentOff
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(setDiscountDto), Encoding.UTF8, "application/json");
            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri($"http://localhost:9050/items/{itemId}/setDiscount"))
                {
                    Content = httpContent
                };

            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);

                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            }
        }

        public static async Task GivenItemIsDiscountedByAFixedAmount(string itemCode, decimal amountOff)
        {
            var getItemRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Get,
                    requestUri: new Uri($"http://localhost:9050/items?code={itemCode}"));

            Guid itemId;

            using (var httpClient = new HttpClient())
            {
                var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

                Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

                var body =
                    JsonConvert.DeserializeAnonymousType(await getItemResponse.Content.ReadAsStringAsync(),
                        new
                        {
                            Id = Guid.Empty
                        });

                itemId = body.Id;
            }

            var setDiscountDto =
                new
                {
                    AmountOff = amountOff
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(setDiscountDto), Encoding.UTF8, "application/json");
            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri($"http://localhost:9050/items/{itemId}/setDiscount"))
                {
                    Content = httpContent
                };

            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);

                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            }
        }

        public static async Task ThenUserCanComputeTotalCostOfShoppingItems(
            List<dynamic> shoppingItems,
            decimal expectedTotalCost)
        {
            var computeCostDto =
                new
                {
                    ShoppingItems =
                        shoppingItems
                            .Select(async x =>
                            {
                                var getItemRequestMessage =
                                    new HttpRequestMessage(
                                        method: HttpMethod.Get,
                                        requestUri: new Uri($"http://localhost:9050/items?code={x.ItemCode}"));

                                Guid itemId;

                                using (var httpClient = new HttpClient())
                                {
                                    var getItemResponse = await httpClient.SendAsync(getItemRequestMessage);

                                    Assert.Equal(HttpStatusCode.OK, getItemResponse.StatusCode);

                                    var body =
                                        JsonConvert.DeserializeAnonymousType(await getItemResponse.Content.ReadAsStringAsync(),
                                            new
                                            {
                                                Id = Guid.Empty
                                            });

                                    itemId = body.Id;
                                }

                                return new
                                {
                                    Id = itemId,
                                    Quantity = x.Quantity
                                };
                            }).Select(x => x.Result)
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
    }
}