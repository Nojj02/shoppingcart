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
    public class ComputeCostTests
    {
        public ComputeCostTests()
        {
            DatabaseHelper.DeleteAllData();
        }

        [Fact]
        public async Task ComputesTotalCostForItems()
        {
            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30},
                    new {Code = "apple", Price = 70},
                    new {Code = "tomato", Price = 50}
                });

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 160);
        }

        [Fact]
        public async Task ItemsCanBeDiscountedByPercentage()
        {
            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30},
                    new {Code = "apple", Price = 70},
                    new {Code = "tomato", Price = 50}
                });

            await Steps.GivenItemIsDiscountedByAPercentage(itemCode: "potato", percentOff: 10);

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 151);
        }

        [Fact]
        public async Task ItemsCanBeDiscountedByAFixedAmount()
        {
            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30},
                    new {Code = "apple", Price = 70},
                    new {Code = "tomato", Price = 50}
                });

            await Steps.GivenItemIsDiscountedByAFixedAmount(itemCode: "apple", amountOff: 30);

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 130);
        }
    }
}