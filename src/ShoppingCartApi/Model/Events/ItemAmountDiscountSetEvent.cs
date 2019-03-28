using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemAmountDiscountSetEvent : IItemEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public decimal AmountOff { get; set; }
    }
}