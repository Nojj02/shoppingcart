using System;

namespace ShoppingCartApi.Model
{
    public class CouponReadModel : ICoupon
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Percent PercentOff { get; set; }
        public Guid? ForItemTypeId { get; set; }

        public static CouponReadModel Map(Coupon x)
        {
            return new CouponReadModel
            {
                Id = x.Id,
                Code = x.Code,
                ForItemTypeId = x.ForItemTypeId,
                PercentOff = x.PercentOff
            };
        }
    }
}