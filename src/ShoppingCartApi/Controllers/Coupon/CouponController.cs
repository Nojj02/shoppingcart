using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.Controllers.Coupon
{
    [Route("coupons")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<ObjectResult> Post([FromBody] PostRequestDto postRequestDto)
        {
            var coupon =
                new Model.Coupon(
                    id: Guid.NewGuid(),
                    code: postRequestDto.Code,
                    amountOff: postRequestDto.AmountOff,
                    percentOff: new Percentage(postRequestDto.PercentOff));

            await _couponRepository.SaveAsync(coupon);

            var dto = MapToDto(coupon);

            var url = Url.Action(nameof(Get), new { id = Guid.NewGuid() });

            return Created(url, dto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ObjectResult> Get([FromBody] Guid id)
        {
            var entity = await _couponRepository.GetAsync(id);

            var dto = MapToDto(entity);

            return Ok(dto);
        }

        private static CouponDto MapToDto(Model.Coupon coupon)
        {
            var dto = new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                AmountOff = coupon.AmountOff,
                PercentOff = coupon.PercentOff.Value
            };
            return dto;
        }
    }
}