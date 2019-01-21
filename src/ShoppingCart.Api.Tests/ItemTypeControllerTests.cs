using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using ShoppingCart.Api.Controllers;
using ShoppingCart.Api.Controllers.ItemType;
using ShoppingCart.Api.Tests.Helpers;
using Xunit;

namespace ShoppingCart.Api.Tests
{
    public class ItemTypeControllerTests
    {
        [Fact]
        public async Task Returns404_GetNonExistentItem()
        {
            var repository = new ItemTypeRepository();
            var itemTypeController = new ItemTypeController(repository);
            BootstrapController(itemTypeController);

            var result = await itemTypeController.GetByItemTypeCode("unknown");

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("unknown", result.Value);
        }

        [Fact]
        public async Task RespondsWithCreated_PostNewItemType()
        {
            var repository = new ItemTypeRepository();
            var itemTypeController = new ItemTypeController(repository);
            BootstrapController(itemTypeController);

            var postNewItemTypeDto = new PostNewItemTypeDto
            {
                Code = "vegetable"
            };

            var result = await itemTypeController.Post(postNewItemTypeDto);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

            var itemTypeDto = (ItemTypeDto)result.Value;

            Assert.Equal("vegetable", itemTypeDto.Code);
        }

        [Fact]
        public async Task CanGetItemType_PostNewItemType()
        {
            var repository = new ItemTypeRepository();
            var itemTypeController = new ItemTypeController(repository);
            BootstrapController(itemTypeController);

            var postNewItemTypeDto = new PostNewItemTypeDto
            {
                Code = "vegetable"
            };

            await itemTypeController.Post(postNewItemTypeDto);

            var anotherItemTypeController = new ItemTypeController(repository);
            BootstrapController(anotherItemTypeController);
            var result = await anotherItemTypeController.GetByItemTypeCode("vegetable");
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var itemTypeDto = (ItemTypeDto)result.Value;

            Assert.Equal("vegetable", itemTypeDto.Code);
        }

        [Fact]
        public async Task CanGetItemTypes_PostMultipleItemTypes()
        {
            var repository = new ItemTypeRepository();
            var itemTypeController = new ItemTypeController(repository);
            BootstrapController(itemTypeController);

            var postNewFruitItemTypeDto = new PostNewItemTypeDto
            {
                Code = "fruit"
            };

            await itemTypeController.Post(postNewFruitItemTypeDto);

            var postNewVegetableItemTypeDto = new PostNewItemTypeDto
            {
                Code = "vegetable"
            };

            await itemTypeController.Post(postNewVegetableItemTypeDto);

            var fruitResult = await itemTypeController.GetByItemTypeCode("fruit");
            Assert.Equal((int)HttpStatusCode.OK, fruitResult.StatusCode);

            var fruitItemTypeDto = (ItemTypeDto)fruitResult.Value;
            Assert.Equal("fruit", fruitItemTypeDto.Code);

            var vegetableResult = await itemTypeController.GetByItemTypeCode("vegetable");
            Assert.Equal((int)HttpStatusCode.OK, vegetableResult.StatusCode);

            var vegetableItemTypeDto = (ItemTypeDto) vegetableResult.Value;

            Assert.Equal("vegetable", vegetableItemTypeDto.Code);
        }

        private void BootstrapController(ItemTypeController itemTypeController)
        {
            itemTypeController.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
