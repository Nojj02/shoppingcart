using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Calculator;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace ShoppingCartApi.Tests
{
    public class CalculatorControllerTests
    {
        [Fact]
        public async Task CostIsZero_NoShoppingItems()
        {
            var itemRepository = new ItemRepository();
            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var result = await calculatorController.ComputeCost(new CalculatorComputeCostRequestDto());
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;
            
            Assert.Equal(0, dto.TotalCost);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { ScenarioReturnsCostOfSingleItem.ShoppingItemDtos, ScenarioReturnsCostOfSingleItem.ExpectedCost },
                new object[] { ScenarioReturnsCostOfTwoItemsWithSingleQuantity.ShoppingItemDtos, ScenarioReturnsCostOfTwoItemsWithSingleQuantity.ExpectedCost }
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task ReturnsTotalCost_ShoppingItems(
            IEnumerable<ShoppingItemDto> shoppingItemDtos,
            decimal expectedCost)
        {
            var itemRepository = new ItemRepository();

            var itemController = new ItemController(itemRepository);
            BootstrapController(itemController);
            
            var postNewPotatoItemDto = new PostRequestDto
            {
                Code = "potato",
                Price = 30
            };
            
            var postNewItemResult = await itemController.Post(postNewPotatoItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewItemResult.StatusCode);
           
            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var calculatorComputeCostRequestDto =
                new CalculatorComputeCostRequestDto
                {
                    ShoppingItems = shoppingItemDtos
                };

            var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;
            
            Assert.Equal(expectedCost, dto.TotalCost);
        }

        public class ScenarioReturnsCostOfSingleItem
        {
            public static List<ShoppingItemDto> ShoppingItemDtos =>
                new List<ShoppingItemDto>
                    {
                        new ShoppingItemDto
                        {
                            ItemCode = "potato",
                            Quantity = 1
                        }
                    };

            public static decimal ExpectedCost => 30;
        }

        public class ScenarioReturnsCostOfTwoItemsWithSingleQuantity
        {
            public static List<ShoppingItemDto> ShoppingItemDtos =>
                new List<ShoppingItemDto>
                {
                    new ShoppingItemDto
                    {
                        ItemCode = "potato",
                        Quantity = 1
                    },
                    new ShoppingItemDto
                    {
                        ItemCode = "lettuce",
                        Quantity = 1
                    }
                };

            public static decimal ExpectedCost => 80;
        }

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
