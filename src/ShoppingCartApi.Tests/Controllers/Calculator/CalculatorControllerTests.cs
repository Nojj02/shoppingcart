using System.Collections.Generic;
using System.Linq;
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

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(0, dto.TotalCost);
            }

            [Fact]
            public async Task ReturnsTotalCost_SingleItem_SingleQuantity()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(30, dto.TotalCost);
            }

            [Fact]
            public async Task ReturnsTotalCost_TwoItems_SingleQuantities()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 1
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(80, dto.TotalCost);
            }

            [Fact]
            public async Task ReturnsTotalCost_TwoItems_DifferentQuantities()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(140, dto.TotalCost);
            }


            [Fact]
            public async Task ReturnsTotalCost_ThreeItems_DifferentQuantities()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var postNewCabbageItemDto = new PostRequestDto
                {
                    Code = "cabbage",
                    Price = 20
                };

                var postCabbageResult = await itemController.Post(postNewCabbageItemDto);
                var cabbageDto = (ItemDto)postCabbageResult.Value;

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                },
                                new ShoppingItemDto
                                {
                                    Id = cabbageDto.Id,
                                    Quantity = 2
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(180, dto.TotalCost);
            }
            
            [Fact]
            public async Task ApplyDiscount_TwoItems_DiscountedOneItemByPercentage()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var setDiscountOnPotatoDto = new SetDiscountRequestDto
                {
                    PercentOff = 10
                };

                await itemController.SetDiscount(potatoDto.Id, setDiscountOnPotatoDto);

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(131, dto.TotalCost);
            }

            [Fact]
            public async Task ApplyDiscount_TwoItems_DiscountedOneItemByFixedAmount()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var setDiscountOnPotatoDto = new SetDiscountRequestDto
                {
                    AmountOff = 5,
                    PercentOff = 50
                };

                await itemController.SetDiscount(potatoDto.Id, setDiscountOnPotatoDto);

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(95, dto.TotalCost);
            }

            [Fact]
            public async Task ApplyHighestDiscount_TwoItems_DiscountedOneItemByFixedAmountAndPercentage()
            {
                var itemRepository = new InMemoryItemRepository();

                var itemController = new ItemController(itemRepository);
                BootstrapController(itemController);

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    Price = 50
                };

                var postLettuceResult = await itemController.Post(postNewLettuceItemDto);
                var lettuceDto = (ItemDto)postLettuceResult.Value;

                var setDiscountOnPotatoDto = new SetDiscountRequestDto
                {
                    AmountOff = 5
                };

                await itemController.SetDiscount(potatoDto.Id, setDiscountOnPotatoDto);

                var calculatorController = new CalculatorController(itemRepository);
                BootstrapController(calculatorController);

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 3
                                },
                                new ShoppingItemDto
                                {
                                    Id = lettuceDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(125, dto.TotalCost);
            }
        }

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
