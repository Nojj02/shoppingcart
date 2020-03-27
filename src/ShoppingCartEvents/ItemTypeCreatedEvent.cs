using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemTypeCreatedEvent : DomainEvent, IItemTypeEvent
    {
        public string Code { get; set; }
    }
}