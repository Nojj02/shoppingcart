﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;
using ShoppingCartSharedKernel;

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
                    percentOff: new Percent(postRequestDto.PercentOff),
                    forItemTypeId: postRequestDto.ForItemTypeId);

            await _couponRepository.SaveAsync(coupon, DateTimeOffset.UtcNow);

            var dto = MapToDto(coupon);

            var url = Url.Action(nameof(Get), new { id = coupon.Id });

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
                PercentOff = coupon.PercentOff.Value
            };
            return dto;
        }
    }
}