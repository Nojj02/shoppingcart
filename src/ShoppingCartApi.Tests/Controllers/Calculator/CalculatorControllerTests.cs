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

        public class ReturnsTotalCostScenarioData
        {
            public List<ShoppingItemDto> ShoppingItems { get; set; }

            public decimal ExpectedTotalCost { get; set; }
        }

        public static TheoryData ReturnsTotalCostScenarios =>
            new TheoryData<string, ReturnsTotalCostScenarioData>
            {
                {
                    "SingleItem_SingleQuantity",
                    new ReturnsTotalCostScenarioData
                    {
                        ShoppingItems = 
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    ItemCode = "potato",
                                    Quantity = 1
                                }
                            },
                        ExpectedTotalCost = 30
                    }
                },
                {
                    "TwoItems_SingleQuantities",
                    new ReturnsTotalCostScenarioData
                    {
                        ShoppingItems = new List<ShoppingItemDto>
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
                        ExpectedTotalCost = 80
                    }
                },
                {
                    "TwoItems_DifferentQuantities",
                    new ReturnsTotalCostScenarioData
                    {
                        ShoppingItems = new List<ShoppingItemDto>
                        {
                            new ShoppingItemDto
                            {
                                ItemCode = "potato",
                                Quantity = 3
                            },
                            new ShoppingItemDto
                            {
                                ItemCode = "lettuce",
                                Quantity = 1
                            }
                        },
                        ExpectedTotalCost = 140
                    }
                },
                {
                    "ThreeItems_DifferentQuantities",
                    new ReturnsTotalCostScenarioData
                    {
                        ShoppingItems = new List<ShoppingItemDto>
                        {
                            new ShoppingItemDto
                            {
                                ItemCode = "potato",
                                Quantity = 3
                            },
                            new ShoppingItemDto
                            {
                                ItemCode = "lettuce",
                                Quantity = 1
                            },
                            new ShoppingItemDto
                            {
                                ItemCode = "cabbage",
                                Quantity = 2
                            }
                        },
                        ExpectedTotalCost = 180
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ReturnsTotalCostScenarios))]
        public async Task ReturnsTotalCost(
            string testDescription,
            ReturnsTotalCostScenarioData scenarioData)
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

            var postNewCabbageItemDto = new PostRequestDto
            {
                Code = "Cabbage",
                Price = 20
            };

            var postNewCabbageItemResult = await itemController.Post(postNewCabbageItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewCabbageItemResult.StatusCode);

            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var calculatorComputeCostRequestDto =
                new CalculatorComputeCostRequestDto
                {
                    ShoppingItems = scenarioData.ShoppingItems
                };

            var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;

            Assert.Equal(scenarioData.ExpectedTotalCost, dto.TotalCost);
        }

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
