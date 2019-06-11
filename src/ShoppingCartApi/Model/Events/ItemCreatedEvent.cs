using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemCreatedEvent : DomainEvent, IItemEvent
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public Guid ItemTypeId { get; set; }
    }
}