using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Calculator;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.Calculator
{
    public class CalculatorControllerTests
    {
        public class ComputeCost : CalculatorControllerTests
        {
            [Fact]
            public async Task CostIsZero_NoShoppingItems()
            {
                var itemRepository = new InMemoryItemRepository();
                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var result = await calculatorController.ComputeCost(new CalculatorComputeCostRequestDto());

                Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto) result.Value;

                Assert.Equal(0, dto.TotalCost);
            }

            public class ReturnsTotalCostScenarioData
            {
                public List<ShoppingItemDto> ShoppingItems { get; set; }

                public decimal ExpectedTotalCost { get; set; }
            }

            [Theory]
            [MemberData(nameof(ReturnsTotalCostScenarios))]
            public async Task ReturnsTotalCost(
                string testDescription,
                ReturnsTotalCostScenarioData scenarioData)
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                await itemController.Post(postNewPotatoItemDto);

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                await itemController.Post(postNewLettuceItemDto);

                var postNewCabbageItemDto = new PostRequestDto
                {
                    Code = "cabbage",
                    Price = 20
                };

                await itemController.Post(postNewCabbageItemDto);

                var postNewDiscountedPotatoItemDto = new PostRequestDto
                {
                    Code = "discounted-potato",
                    Price = 30
                };

                await itemController.Post(postNewDiscountedPotatoItemDto);

                var setDiscountOnPotatoDto = new SetDiscountRequestDto
                {
                    PercentOff = 10
                };

                await itemController.SetDiscount("discounted-potato", setDiscountOnPotatoDto);

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems = scenarioData.ShoppingItems
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto) result.Value;

                Assert.Equal(scenarioData.ExpectedTotalCost, dto.TotalCost);
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
                    },
                    {
                        "TwoItems_WithDiscountedItems",
                        new ReturnsTotalCostScenarioData
                        {
                            ShoppingItems = new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    ItemCode = "discounted-potato",
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    ItemCode = "lettuce",
                                    Quantity = 1
                                }
                            },
                            ExpectedTotalCost = 131
                        }
                    }
                };
        }

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
