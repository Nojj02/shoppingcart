using System;
using ShoppingCartSharedKernel;

namespace ShoppingCartReader.Model
{
    public class CouponReadModel : IEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Percent PercentOff { get; set; }
        public Guid? ForItemTypeId { get; set; }
    }
}