using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Calculator;
using ShoppingCartApi.Controllers.Coupon;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Controllers.ItemType;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;
using ShoppingCartApi.Tests.Helpers;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;
using Xunit;
using PostRequestDto = ShoppingCartApi.Controllers.Item.PostRequestDto;

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
                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
            public async Task ApplyDiscount_FromCoupon()
            {
                var itemRepository = new InMemoryItemRepository();
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var couponRepository = new InMemoryCouponRepository();
                var couponReadRepository = new InMemoryCouponReadRepository();
                couponRepository.EventOccurred += entity => couponReadRepository.UpdateAsync(new CouponReadModel()); // This is broke

                var couponController = new CouponController(couponRepository)
                    .BootstrapForTests();

                var postRequestDto = new ShoppingCartApi.Controllers.Coupon.PostRequestDto
                {
                    Code = "GRAND_SALE",
                    PercentOff = 20
                };

                await couponController.Post(postRequestDto);

                var calculatorController = 
                    new CalculatorController(itemRepository, couponReadRepository)
                        .BootstrapForTests();

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        CouponCode = "GRAND_SALE",
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

                Assert.Equal(112, dto.TotalCost);
            }

            [Fact]
            public async Task ApplyDiscount_FromCouponOnlyForSelectItemType()
            {
                var itemTypeRepository = new InMemoryItemTypeRepository();
                var itemTypeReadRepository = new InMemoryItemTypeReadRepository();
                itemTypeRepository.EventOccurred += entity => itemTypeReadRepository.UpdateAsync(new ItemTypeReadModel
                {
                    Id = entity.Id,
                    Code = entity.Code
                });
                var itemRepository = new InMemoryItemRepository();
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });
                var couponRepository = new InMemoryCouponRepository();
                var couponReadRepository = new InMemoryCouponReadRepository();
                couponRepository.EventOccurred += entity => couponReadRepository.UpdateAsync(new CouponReadModel()); // This is broke

                var itemTypeController = new ItemTypeController(itemTypeRepository, itemTypeReadRepository)
                    .BootstrapForTests();

                var postFruitResult = await itemTypeController.Post(
                    new ShoppingCartApi.Controllers.ItemType.PostRequestDto
                    {
                        Code = "fruit"
                    });
                var fruitItemTypeDto = (ItemTypeDto) postFruitResult.Value;

                var postVegetableResult = await itemTypeController.Post(
                    new ShoppingCartApi.Controllers.ItemType.PostRequestDto
                    {
                        Code = "vegetable"
                    });
                var vegetableItemTypeDto = (ItemTypeDto) postVegetableResult.Value;

                var itemController = new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

                var postPotatoResult = await itemController.Post(new PostRequestDto
                {
                    Code = "potato",
                    Price = 30,
                    ItemTypeId = vegetableItemTypeDto.Id
                });
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var postAppleResult = await itemController.Post(new PostRequestDto
                {
                    Code = "apple",
                    Price = 50,
                    ItemTypeId = fruitItemTypeDto.Id
                });
                var appleDto = (ItemDto)postAppleResult.Value;
                
                var couponController = new CouponController(couponRepository)
                    .BootstrapForTests();
                var postRequestDto = new ShoppingCartApi.Controllers.Coupon.PostRequestDto
                {
                    Code = "HALF_OFF_FRUIT",
                    PercentOff = 50,
                    ForItemTypeId = fruitItemTypeDto.Id
                };
                await couponController.Post(postRequestDto);

                var calculatorController =
                    new CalculatorController(itemRepository, couponReadRepository)
                        .BootstrapForTests();

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        CouponCode = "HALF_OFF_FRUIT",
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
                                    Id = appleDto.Id,
                                    Quantity = 1
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(115, dto.TotalCost);
            }

            [Theory]
            [InlineData("CouponIsHighest", 10, 5, 20, 80)]
            [InlineData("ItemPercentIsHighest", 30, 5, 20, 70)]
            [InlineData("AmountOffIsHighest", 10, 22.50, 20, 55)]
            public async Task ApplyHighestDiscount_HasAmountOffHasPercentOffHasCoupon(
                string testName,
                double itemPercentOff,
                decimal itemAmountOff,
                double couponPercentOff,
                decimal expectedTotalCost)
            {
                var itemRepository = new InMemoryItemRepository();
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 50
                };

                var postPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postPotatoResult.Value;

                var setDiscountOnPotatoDto = new SetDiscountRequestDto
                {
                    PercentOff = itemPercentOff,
                    AmountOff = itemAmountOff
                };

                await itemController.SetDiscount(potatoDto.Id, setDiscountOnPotatoDto);

                var couponRepository = new InMemoryCouponRepository();
                var couponReadRepository = new InMemoryCouponReadRepository();
                couponRepository.EventOccurred += entity => couponReadRepository.UpdateAsync(new CouponReadModel()); // This is broke

                var couponController = new CouponController(couponRepository)
                    .BootstrapForTests();

                var postRequestDto = new ShoppingCartApi.Controllers.Coupon.PostRequestDto
                {
                    Code = "GRAND_SALE",
                    PercentOff = couponPercentOff
                };

                await couponController.Post(postRequestDto);

                var calculatorController =
                    new CalculatorController(itemRepository, couponReadRepository)
                        .BootstrapForTests();

                var calculatorComputeCostRequestDto =
                    new CalculatorComputeCostRequestDto
                    {
                        CouponCode = "GRAND_SALE",
                        ShoppingItems =
                            new List<ShoppingItemDto>
                            {
                                new ShoppingItemDto
                                {
                                    Id = potatoDto.Id,
                                    Quantity = 2
                                }
                            }
                    };

                var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.NotNull(result.Value);

                var dto = (CalculatorComputeCostDto)result.Value;

                Assert.Equal(expectedTotalCost, dto.TotalCost);
            }

            [Fact]
            public async Task ApplyHighestDiscount_TwoItems_DiscountedOneItemByFixedAmountAndPercentage()
            {
                var itemRepository = new InMemoryItemRepository();
                var itemReadRepository = new InMemoryItemReadRepository();
                itemRepository.EventOccurred += entity => itemReadRepository.UpdateAsync(new ItemReadModel
                {
                    // TODO: This is still broke
                });

                var itemController =
                    new ItemController(itemRepository, itemReadRepository)
                    .BootstrapForTests();

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

                var calculatorController = new CalculatorController(itemRepository, new InMemoryCouponReadRepository())
                    .BootstrapForTests();

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
    }
}
