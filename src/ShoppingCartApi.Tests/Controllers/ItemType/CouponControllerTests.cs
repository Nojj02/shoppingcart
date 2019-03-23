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
                var couponRepository = new InMemoryItemTypeRepository();

                var couponController =
                    new ItemTypeController(couponRepository)
                        .BootstrapForTests(urlHelper: new ActionNameOnlyUrlHelper());

                var postRequestDto = new PostRequestDto
                {
                    Code = "fruit"
                };

                var postResponse = (CreatedResult)await couponController.Post(postRequestDto);

                Assert.Equal((int)HttpStatusCode.Created, postResponse.StatusCode);

                var couponDto = (ItemTypeDto) postResponse.Value;

                Assert.Equal(nameof(ItemTypeController.Get), postResponse.Location);

                Assert.Equal("fruit", couponDto.Code);
            }
        }
    }
}
