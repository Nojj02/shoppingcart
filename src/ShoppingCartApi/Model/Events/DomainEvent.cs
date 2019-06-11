using System;

namespace ShoppingCartApi.Model.Events
{
    public class DomainEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}