using System;

namespace ShoppingCartHandlers
{
    public class ItemTypeCreatedEvent
    {
        public Guid Id { get; set; }
        
        public int Version { get; set; }
        
        public string Code { get; set; }
    }
}