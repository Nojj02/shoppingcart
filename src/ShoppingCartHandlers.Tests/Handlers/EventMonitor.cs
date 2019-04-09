using System;
using System.Collections.Generic;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventMonitor
    {
        private readonly IApiHelper _apiHelper;

        private readonly List<(string ResourceName, Type EventType, IEventHandler Handler)> _eventSubscriptions = new List<(string, Type, IEventHandler)>();

        public EventMonitor(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public void Subscribe<TEvent>(string resourceName, IEventHandler handler)
        {
            _eventSubscriptions.Add((resourceName, typeof(TEvent), handler));
        }

        public void Poll()
        {
            foreach (var subscription in _eventSubscriptions)
            {
                var newEvents = _apiHelper.GetNewEvents(subscription.ResourceName);
                subscription.Handler.Handle(newEvents);
            }
        }
    }
}