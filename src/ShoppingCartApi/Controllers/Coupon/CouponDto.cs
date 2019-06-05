using System;
using System.Collections.Generic;

namespace ShoppingCartApi.Controllers.Coupon
{
    public class CouponDto
    {
        public string Code { get; set; }
        public double PercentOff { get; set; }
        public Guid Id { get; set; }
    }
}