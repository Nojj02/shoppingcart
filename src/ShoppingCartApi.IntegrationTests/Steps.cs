using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingCartApi.IntegrationTests.Api;
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

        public static async Task GivenItemIsDiscounted(string itemCode, double percentOff, decimal amountOff)
        {
            var item = await ItemApi.GetByItemCodeAsync(itemCode);

            var setDiscountDto =
                new
                {
                    PercentOff = percentOff,
                    AmountOff = amountOff
                };

            var httpContent = new StringContent(JsonConvert.SerializeObject(setDiscountDto), Encoding.UTF8, "application/json");
            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri($"http://localhost:9050/items/{item.Id}/setDiscount"))
                {
                    Content = httpContent
                };

            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);

                Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            }
        }

        public static async Task GivenACoupon(string couponCode, double percentOff)
        {
            var couponDto = new
            {
                Code = couponCode,
                PercentOff = percentOff
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(couponDto), Encoding.UTF8, "application/json");

            var postRequestMessage =
                new HttpRequestMessage(
                    method: HttpMethod.Post,
                    requestUri: new Uri("http://localhost:9050/coupons"))
                {
                    Content = httpContent
                };

            using (var httpClient = new HttpClient())
            {
                var postResponse = await httpClient.SendAsync(postRequestMessage);

                Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            }
        }

        public static async Task ThenUserCanComputeTotalCostOfShoppingItems(
            List<dynamic> shoppingItems,
            decimal expectedTotalCost,
            string couponCode = "")
        {
            var computeCostDto =
                new
                {
                    CouponCode = couponCode,
                    ShoppingItems =
                        shoppingItems
                            .Select(async x =>
                            {
                                var item = await ItemApi.GetByItemCodeAsync(x.ItemCode);

                                return new
                                {
                                    Id = item.Id,
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