using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventMonitor
    {
        private readonly IEventApi _eventApi;
        private readonly IEventTrackingRepository _eventTrackingRepository;

        private readonly List<(string ResourceName, Type EventType, IEventHandler Handler)> _eventSubscriptions = new List<(string, Type, IEventHandler)>();

        public EventMonitor(IEventApi eventApi, IEventTrackingRepository eventTrackingRepository = null)
        {
            _eventApi = eventApi;
            _eventTrackingRepository = eventTrackingRepository;
        }

        public void Subscribe<TEvent>(string resourceName, IEventHandler handler)
        {
            _eventSubscriptions.Add((resourceName, typeof(TEvent), handler));
        }

        public async Task Poll()
        {
            foreach (var subscription in _eventSubscriptions)
            {
                var lastMessageNumber = -1;
                if (_eventTrackingRepository != null)
                {
                    lastMessageNumber = _eventTrackingRepository.GetLastMessageNumber(subscription.ResourceName);
                }

                var newEvents = await _eventApi.GetEventsAfterAsync(subscription.ResourceName, lastMessageNumber);
                subscription.Handler.Handle(newEvents);
            }
        }
    }
}