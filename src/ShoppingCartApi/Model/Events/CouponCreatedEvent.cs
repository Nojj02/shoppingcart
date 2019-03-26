using System;

namespace ShoppingCartApi.Model.Events
{
    public class CouponCreatedEvent : ICouponEvent
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Percent PercentOff { get; set; }
        public Guid? ForItemTypeId { get; set; }
    }
}