using System;
using ShoppingCartSharedKernel;

namespace ShoppingCartEvents
{
    public class CouponCreatedEvent : DomainEvent, ICouponEvent
    {
        public string Code { get; set; }
        public Percent PercentOff { get; set; }
        public Guid? ForItemTypeId { get; set; }
    }
}