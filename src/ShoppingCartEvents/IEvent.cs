using System;

namespace ShoppingCartEvents
{
    public interface IEvent
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}