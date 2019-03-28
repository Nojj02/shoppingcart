using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemPercentageDiscountSetEvent : IItemEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public Percent PercentOff { get; set; }
    }
}