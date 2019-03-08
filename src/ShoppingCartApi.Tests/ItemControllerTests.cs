using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests
{
    public class ItemControllerTests
    {
        [Fact]
        public async Task Returns404_GetNonExistentItem()
        {
            var repository = new ItemRepository();
            var itemController = new ItemController(repository);
            BootstrapController(itemController);

            var result = await itemController.GetByItemCode("unknown");

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("unknown", result.Value);
        }

        [Fact]
        public async Task RespondsWithCreated_PostNewItem()
        {
            var repository = new ItemRepository();
            var itemController = new ItemController(repository);
            BootstrapController(itemController);

            var postNewItemDto = new PostNewItemDto
            {
                Code = "lettuce"
            };

            var result = await itemController.Post(postNewItemDto);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

            var itemDto = (ItemDto)result.Value;

            Assert.Equal("lettuce", itemDto.Code);
        }

        [Fact]
        public async Task CanGetItem_PostNewItem()
        {
            var repository = new ItemRepository();
            var itemController = new ItemController(repository);
            BootstrapController(itemController);

            var postNewItemDto = new PostNewItemDto
            {
                Code = "lettuce"
            };

            await itemController.Post(postNewItemDto);

            var anotherItemController = new ItemController(repository);
            BootstrapController(anotherItemController);
            var result = await anotherItemController.GetByItemCode("lettuce");
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var itemDto = (ItemDto)result.Value;

            Assert.Equal("lettuce", itemDto.Code);
        }

        [Fact]
        public async Task CanGetItems_PostMultipleItems()
        {
            var repository = new ItemRepository();
            var itemController = new ItemController(repository);
            BootstrapController(itemController);

            var postNewPotatoItemDto = new PostNewItemDto
            {
                Code = "potato",
                Price = 30
            };

            await itemController.Post(postNewPotatoItemDto);

            var postNewLettuceItemDto = new PostNewItemDto
            {
                Code = "lettuce",
                Price = 50
            };

            await itemController.Post(postNewLettuceItemDto);

            var potatoResult = await itemController.GetByItemCode("potato");
            Assert.Equal((int)HttpStatusCode.OK, potatoResult.StatusCode);

            var potatoItemDto = (ItemDto)potatoResult.Value;
            Assert.Equal("potato", potatoItemDto.Code);
            Assert.Equal(30, potatoItemDto.Price);

            var lettuceResult = await itemController.GetByItemCode("lettuce");
            Assert.Equal((int)HttpStatusCode.OK, lettuceResult.StatusCode);

            var lettuceItemDto = (ItemDto) lettuceResult.Value;

            Assert.Equal("lettuce", lettuceItemDto.Code);
            Assert.Equal(50, lettuceItemDto.Price);
        }

        private void BootstrapController(ItemController itemController)
        {
            itemController.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
