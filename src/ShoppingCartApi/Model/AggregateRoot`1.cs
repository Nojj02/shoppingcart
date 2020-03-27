using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using ShoppingCartEvents;

namespace ShoppingCartApi.Model
{
    public abstract class AggregateRoot<TEvent> : AggregateRoot, IAggregateRoot<TEvent>
        where TEvent : IEvent
    {
        private readonly List<TEvent> _events = new List<TEvent>();
        private readonly List<TEvent> _newEvents = new List<TEvent>();

        protected AggregateRoot(Guid id)
            : base(id)
        {
        }

        protected AggregateRoot(Guid id, IReadOnlyList<TEvent> events)
            : this(id)
        {
            foreach (var anEvent in events)
            {
                ApplyEventsOnConstruction(anEvent);
            }
        }
        
        public IReadOnlyList<TEvent> Events => _events;

        public IReadOnlyList<TEvent> NewEvents => _newEvents;

        public int CurrentVersion => _events.Any() ? _events.Max(x => x.Version) : 0;

        /// <summary>
        /// Called during reconstitution of an object from its events.
        /// Inheritors decide how to apply an event.
        /// TODO: This is a virtual call in a constructor. If we can guarantee that the Creation event always gets called first and all initialization is guaranteed performed on that step, then this should be safe.
        /// </summary>
        /// <param name="anEvent"></param>
        protected abstract void ApplyEventsOnConstruction(TEvent anEvent);

        protected void AddEvent(TEvent anEvent, bool isNew)
        {
            _events.Add(anEvent);
            if (isNew)
            {
                _newEvents.Add(anEvent);
            }
        }

        public class UnsupportedEventException : Exception
        {
            public UnsupportedEventException(Type eventType)
                : base($"Event of type {eventType.FullName} is not supported by this class.")
            {

            }
        }
    }
}