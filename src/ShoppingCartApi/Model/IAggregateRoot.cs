using System.Collections.Generic;
using ShoppingCartEvents;

namespace ShoppingCartApi.Model
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<out TEvent> : IAggregateRoot
        where TEvent : IEvent
    {
        IReadOnlyList<TEvent> Events { get; }
        IReadOnlyList<TEvent> NewEvents { get; }
    }
}