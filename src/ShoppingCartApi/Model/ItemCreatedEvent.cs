using System;

namespace ShoppingCartApi.Model
{
    public class ItemCreatedEvent : IItemEvent
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public Guid ItemTypeId { get; set; }
    }
}