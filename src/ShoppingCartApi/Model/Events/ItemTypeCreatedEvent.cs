using System;

namespace ShoppingCartApi.Model.Events
{
    public class ItemTypeCreatedEvent : IItemTypeEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Code { get; set; }
    }
}