using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemAmountDiscountSetEvent : DomainEvent, IItemEvent
    {
        public decimal AmountOff { get; set; }
    }
}