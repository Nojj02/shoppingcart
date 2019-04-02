using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Controllers.ItemType;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.ItemType
{
    public class ItemTypeControllerTests
    {
        public class Post : ItemTypeControllerTests
        {
            [Fact]
            public async Task RespondsWithCreated_PostNewItemType()
            {
                var itemTypeRepository = new InMemoryItemTypeRepository();

                var itemTypeController =
                    new ItemTypeController(itemTypeRepository, itemTypeRepository)
                        .BootstrapForTests(urlHelper: new ActionNameOnlyUrlHelper());

                var postRequestDto = new PostRequestDto
                {
                    Code = "fruit"
                };

                var postResponse = (CreatedResult)await itemTypeController.Post(postRequestDto);

                Assert.Equal((int)HttpStatusCode.Created, postResponse.StatusCode);

                var itemTypeDto = (ItemTypeDto) postResponse.Value;

                Assert.Equal(nameof(ItemTypeController.Get), postResponse.Location);

                Assert.Equal("fruit", itemTypeDto.Code);
            }
        }
    }
}
