using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Controllers.Coupon;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.Coupon
{
    public class CouponControllerTests
    {
        public class Post : CouponControllerTests
        {
            [Fact]
            public async Task RespondsWithCreated_PostNewCoupon()
            {
                var couponRepository = new InMemoryCouponRepository();

                var couponController =
                    new CouponController(couponRepository)
                        .BootstrapForTests(urlHelper: new ActionNameOnlyUrlHelper());

                var postRequestDto = new PostRequestDto
                {
                    Code = "GRAND_SALE",
                    PercentOff = 50,
                    AmountOff = 5
                };

                var postResponse = (CreatedResult)await couponController.Post(postRequestDto);

                Assert.Equal((int)HttpStatusCode.Created, postResponse.StatusCode);

                var couponDto = (CouponDto) postResponse.Value;

                Assert.Equal(nameof(CouponController.Get), postResponse.Location);

                Assert.Equal("GRAND_SALE", couponDto.Code);
                Assert.Equal(50, couponDto.PercentOff);
                Assert.Equal(5, couponDto.AmountOff);
            }

            [Fact]
            public async Task CanGetCoupon_PostNewCoupon()
            {
                var couponRepository = new InMemoryCouponRepository();

                var couponController = 
                    new CouponController(couponRepository)
                        .BootstrapForTests();

                var postRequestDto = new PostRequestDto
                {
                    Code = "GRAND_SALE",
                    PercentOff = 50,
                    AmountOff = 5
                };

                var postResponse = await couponController.Post(postRequestDto);

                var postResponseCouponDto = (CouponDto)postResponse.Value;

                var getResponse = await couponController.Get(postResponseCouponDto.Id);

                Assert.Equal((int)HttpStatusCode.OK, getResponse.StatusCode);

                var couponDto = (CouponDto)getResponse.Value;

                Assert.Equal("GRAND_SALE", couponDto.Code);
                Assert.Equal(50, couponDto.PercentOff);
                Assert.Equal(5, couponDto.AmountOff);
            }
        }
    }
}
