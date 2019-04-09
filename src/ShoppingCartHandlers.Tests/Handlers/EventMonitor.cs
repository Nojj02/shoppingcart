using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventMonitor
    {
        private readonly IEventApi _eventApi;

        private readonly List<(string ResourceName, Type EventType, IEventHandler Handler)> _eventSubscriptions = new List<(string, Type, IEventHandler)>();

        public EventMonitor(IEventApi eventApi)
        {
            _eventApi = eventApi;
        }

        public void Subscribe<TEvent>(string resourceName, IEventHandler handler)
        {
            _eventSubscriptions.Add((resourceName, typeof(TEvent), handler));
        }

        public async Task Poll()
        {
            foreach (var subscription in _eventSubscriptions)
            {
                var newEvents = await _eventApi.GetNewEventsAsync(subscription.ResourceName);
                subscription.Handler.Handle(newEvents);
            }
        }
    }
}