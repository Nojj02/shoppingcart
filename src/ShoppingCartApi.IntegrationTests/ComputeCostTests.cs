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
            await Steps.GivenTheItemTypes(
                itemTypes: new List<dynamic>
                {
                    new {Code = "fruit"},
                    new {Code = "vegetable"}
                });

            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30, ItemTypeCode = "vegetable"},
                    new {Code = "apple", Price = 70, ItemTypeCode = "fruit"}
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
        public async Task ItemsCanBeDiscounted()
        {
            await Steps.GivenTheItemTypes(
                itemTypes: new List<dynamic>
                {
                    new {Code = "fruit"},
                    new {Code = "vegetable"}
                });

            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30, ItemTypeCode = "vegetable"},
                    new {Code = "apple", Price = 70, ItemTypeCode = "fruit"},
                    new {Code = "tomato", Price = 50, ItemTypeCode = "fruit"},
                    new {Code = "lettuce", Price = 20, ItemTypeCode = "vegetable"}
                });

            await Steps.GivenItemIsDiscounted(itemCode: "potato", percentOff: 10, amountOff: 0);

            await Steps.GivenItemIsDiscounted(itemCode: "apple", percentOff: 0, amountOff: 30);

            await Steps.GivenItemIsDiscounted(itemCode: "lettuce", percentOff: 50, amountOff: 5);

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1},
                    new {ItemCode = "lettuce", Quantity = 2}
                },
                expectedTotalCost: 141);
        }

        [Fact]
        public async Task ItemsCanBeDiscountedWithACoupon()
        {
            await Steps.GivenTheItemTypes(
                itemTypes: new List<dynamic>
                {
                    new {Code = "fruit"},
                    new {Code = "vegetable"}
                });

            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30, ItemTypeCode = "vegetable"},
                    new {Code = "apple", Price = 70, ItemTypeCode = "fruit"},
                    new {Code = "lettuce", Price = 20, ItemTypeCode = "vegetable"}
                });

            await Steps.GivenACoupon(couponCode: "GRAND_SALE", percentOff: 10);

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1},
                    new {ItemCode = "lettuce", Quantity = 2}
                },
                couponCode: "GRAND_SALE",
                expectedTotalCost: 180);
        }

        [Fact]
        public async Task ItemsCanBeDiscountedWithACouponForASingleTypeOfItem()
        {
            await Steps.GivenTheItemTypes(
                itemTypes: new List<dynamic>
                {
                    new {Code = "fruit"},
                    new {Code = "vegetable"}
                });
            
            await Steps.GivenAShopWithItems(
                items: new List<dynamic>
                {
                    new {Code = "potato", Price = 30, ItemTypeCode = "vegetable"},
                    new {Code = "apple", Price = 70, ItemTypeCode = "fruit"},
                    new {Code = "lettuce", Price = 20, ItemTypeCode = "vegetable"}
                });

            await Steps.GivenACoupon(couponCode: "HALF_OFF_FRUITS", percentOff: 50, itemTypeCode: "fruit");

            await Steps.ThenUserCanComputeTotalCostOfShoppingItems(
                shoppingItems: new List<dynamic>
                {
                    new {ItemCode = "potato", Quantity = 3},
                    new {ItemCode = "apple", Quantity = 1},
                    new {ItemCode = "lettuce", Quantity = 2}
                },
                couponCode: "HALF_OFF_FRUITS",
                expectedTotalCost: 165);
        }
    }
}