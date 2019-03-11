using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Controllers.Calculator;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.Calculator
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

        public class MyTheoryData : TheoryData<List<ShoppingItemDto>, decimal>
        {
            public void AddTheory(
                string testDescription,
                List<ShoppingItemDto> shoppingItems, 
                decimal expectedTotalCost)
            {
                AddRow(testDescription, shoppingItems, expectedTotalCost);
            }
        }

        public static MyTheoryData ReturnsTotalCostScenarios
        {
            get
            {
                var data = new MyTheoryData();
                data.AddTheory(
                    testDescription: "SingleItem_SingleQuantity",
                    shoppingItems: new List<ShoppingItemDto>
                    {
                        new ShoppingItemDto
                        {
                            ItemCode = "potato",
                            Quantity = 1
                        }
                    },
                    expectedTotalCost: 30);
                data.AddTheory(
                    testDescription: "TwoItems_SingleQuantities",
                    shoppingItems: new List<ShoppingItemDto>
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
                    },
                    expectedTotalCost: 80);
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(ReturnsTotalCostScenarios))]
        public async Task ReturnsTotalCost(
            string testDescription,
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

            var postNewPotatoItemResult = await itemController.Post(postNewPotatoItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewPotatoItemResult.StatusCode);

            var postNewLettuceItemDto = new PostRequestDto
            {
                Code = "lettuce",
                Price = 50
            };

            var postNewLettuceItemResult = await itemController.Post(postNewLettuceItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewLettuceItemResult.StatusCode);

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

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
