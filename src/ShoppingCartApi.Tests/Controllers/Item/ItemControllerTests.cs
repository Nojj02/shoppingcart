using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;
using ShoppingCartApi.Tests.Helpers;
using ShoppingCartHandlers.Handlers;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.Item
{
    public class ItemControllerTests
    {
        public class GetByItemCode : ItemControllerTests
        {
            [Fact]
            public async Task Returns404_GetNonExistentItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += entity => readRepository.UpdateAsync(new ItemReadModel());

                var itemController = 
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var result = await itemController.GetByItemCode("unknown");

                Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
                Assert.Equal("unknown", result.Value);
            }
        }

        public class Post : ItemControllerTests
        {
            [Fact]
            public async Task RespondsWithCreated_PostNewItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var itemTypeId = Guid.NewGuid();

                var postNewItemDto = new PostRequestDto
                {
                    Code = "lettuce",
                    ItemTypeId = itemTypeId
                };

                var result = await itemController.Post(postNewItemDto);
                Assert.Equal((int) HttpStatusCode.Created, result.StatusCode);

                var itemDto = (ItemDto) result.Value;

                Assert.Equal("lettuce", itemDto.Code);
                Assert.Equal(itemTypeId, itemDto.ItemTypeId);
            }

            [Fact]
            public async Task CanGetItem_PostNewItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewItemDto = new PostRequestDto
                {
                    Code = "lettuce"
                };

                await itemController.Post(postNewItemDto);

                var anotherItemController = 
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();
                var result = await anotherItemController.GetByItemCode("lettuce");
                Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);

                var itemDto = (ItemDto) result.Value;

                Assert.Equal("lettuce", itemDto.Code);
            }

            [Fact]
            public async Task GetReturnsTheSameItem_PostsTheSameItemTwiceButUsesDataOfTheFirst()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                await itemController.Post(postNewPotatoItemDto);

                var postNewLettuceItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 50
                };

                await itemController.Post(postNewLettuceItemDto);

                var potatoResult = await itemController.GetByItemCode("potato");
                Assert.Equal((int) HttpStatusCode.OK, potatoResult.StatusCode);

                var potatoItemDto = (ItemDto) potatoResult.Value;
                Assert.Equal("potato", potatoItemDto.Code);
                Assert.Equal(30, potatoItemDto.Price);
            }

            [Fact]
            public async Task CanGetItems_PostMultipleItems()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

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

                var potatoResult = await itemController.GetByItemCode("potato");
                Assert.Equal((int) HttpStatusCode.OK, potatoResult.StatusCode);

                var potatoItemDto = (ItemDto) potatoResult.Value;
                Assert.Equal("potato", potatoItemDto.Code);
                Assert.Equal(30, potatoItemDto.Price);

                var lettuceResult = await itemController.GetByItemCode("lettuce");
                Assert.Equal((int) HttpStatusCode.OK, lettuceResult.StatusCode);

                var lettuceItemDto = (ItemDto) lettuceResult.Value;

                Assert.Equal("lettuce", lettuceItemDto.Code);
                Assert.Equal(50, lettuceItemDto.Price);
            }
        }

        public class SetDiscount : ItemControllerTests
        {
            [Fact]
            public async Task ReturnsOk_SetsDiscountOnItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postNewPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postNewPotatoResult.Value;

                var postSetDiscountDto = new SetDiscountRequestDto
                {
                    PercentOff = 10
                };

                var result = await itemController.SetDiscount(potatoDto.Id, postSetDiscountDto);

                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

                var potatoItemDto = (ItemDto) result.Value;
                Assert.Equal(10, potatoItemDto.PercentOff);
            }

            [Fact]
            public async Task CanGetItemWithDiscount_SetsDiscountOnItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postNewPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postNewPotatoResult.Value;

                var postSetDiscountDto = new SetDiscountRequestDto
                {
                    PercentOff = 10
                };

                await itemController.SetDiscount(potatoDto.Id, postSetDiscountDto);

                var potatoResult = await itemController.GetByItemCode("potato");
                Assert.Equal((int)HttpStatusCode.OK, potatoResult.StatusCode);

                var potatoItemDto = (ItemDto)potatoResult.Value;
                Assert.Equal("potato", potatoItemDto.Code);
                Assert.Equal(10, potatoItemDto.PercentOff);
            }

            [Fact]
            public async Task CanGetItemWithDiscount_SetsAmountDiscountOnItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postNewPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postNewPotatoResult.Value;

                var postSetDiscountDto = new SetDiscountRequestDto
                {
                    AmountOff = 5
                };

                await itemController.SetDiscount(potatoDto.Id, postSetDiscountDto);

                var potatoResult = await itemController.GetByItemCode("potato");
                Assert.Equal((int)HttpStatusCode.OK, potatoResult.StatusCode);

                var potatoItemDto = (ItemDto)potatoResult.Value;
                Assert.Equal("potato", potatoItemDto.Code);
                Assert.Equal(5, potatoItemDto.AmountOff);
            }

            [Fact]
            public async Task CanGetItemWithDiscount_SetsAmountDiscountAndPercentageDiscountOnItem()
            {
                var repository = new InMemoryItemRepository();
                var readRepository = new InMemoryItemReadRepository();
                repository.EventOccurred += async entity => await new ItemReadModelHandler(readRepository).Handle(entity.NewEvents.ToList<object>());

                var itemController =
                    new ItemController(repository, readRepository)
                        .BootstrapForTests();

                var postNewPotatoItemDto = new PostRequestDto
                {
                    Code = "potato",
                    Price = 30
                };

                var postNewPotatoResult = await itemController.Post(postNewPotatoItemDto);
                var potatoDto = (ItemDto)postNewPotatoResult.Value;

                var postSetDiscountDto = new SetDiscountRequestDto
                {
                    AmountOff = 5,
                    PercentOff = 10
                };

                await itemController.SetDiscount(potatoDto.Id, postSetDiscountDto);

                var potatoResult = await itemController.GetByItemCode("potato");
                Assert.Equal((int)HttpStatusCode.OK, potatoResult.StatusCode);

                var potatoItemDto = (ItemDto)potatoResult.Value;
                Assert.Equal("potato", potatoItemDto.Code);
                Assert.Equal(5, potatoItemDto.AmountOff);
                Assert.Equal(10, potatoItemDto.PercentOff);
            }
        }
    }
}
