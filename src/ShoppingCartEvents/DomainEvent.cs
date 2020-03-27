using System;

namespace ShoppingCartEvents
{
    public class DomainEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}