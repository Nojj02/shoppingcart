using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartHandlers.DataAccess;
using ShoppingCartHandlers.Handlers;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers
{
    public class EventMonitor
    {
        private readonly IEventApi _eventApi;
        private readonly IEventTrackingRepository _eventTrackingRepository;

        private readonly List<(string ResourceName, Type EventType, IEventHandler Handler)> _eventSubscriptions = new List<(string, Type, IEventHandler)>();

        public EventMonitor(IEventApi eventApi, IEventTrackingRepository eventTrackingRepository)
        {
            _eventApi = eventApi;
            _eventTrackingRepository = eventTrackingRepository;
        }

        public void Subscribe<TEvent>(string resourceName, IEventHandler handler)
        {
            _eventSubscriptions.Add((resourceName, typeof(TEvent), handler));
        }

        public void Subscribe<TEvent>(string resourceName, params IEventHandler[] handlers)
        {
            foreach (var handler in handlers)
            {
                Subscribe<TEvent>(resourceName, handler);
            }
        }

        public async Task Poll()
        {
            var subscriptionsByResourceAndEvent = _eventSubscriptions.GroupBy(x => new { x.ResourceName, x.EventType });
            foreach (var subscriptionByResourceAndEventGroup in subscriptionsByResourceAndEvent)
            {
                var resourceName = subscriptionByResourceAndEventGroup.Key.ResourceName;
                var lastMessageNumber = await _eventTrackingRepository.GetLastMessageNumber(resourceName);
                var newEvents = await _eventApi.GetEventsAfterAsync(resourceName, lastMessageNumber);

                foreach (var subscription in subscriptionByResourceAndEventGroup)
                {
                    await subscription.Handler.Handle(newEvents);
                }
                await _eventTrackingRepository.UpdateLastMessageNumberAsync(
                    resourceName: resourceName,
                    newLastMessageNumber: lastMessageNumber.AddMessageCount(newEvents.Count));
            }
        }
    }
}