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

            await Steps.UserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 160);
        }

        [Fact]
        public async Task ItemsCanBeDiscounted()
        {
            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30},
                    new {Code = "apple", Price = 70},
                    new {Code = "tomato", Price = 50}
                });

            await Steps.GivenItemIsDiscounted(itemCode: "potato", percentOff: 10);

            await Steps.UserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1}
                },
                expectedTotalCost: 151);
        }
    }
}