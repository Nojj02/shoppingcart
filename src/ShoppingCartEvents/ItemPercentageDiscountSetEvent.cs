using System;
using ShoppingCartSharedKernel;

namespace ShoppingCartApi.Model.Events
{
    public class ItemPercentageDiscountSetEvent : DomainEvent, IItemEvent
    {
        public Percent PercentOff { get; set; }
    }
}