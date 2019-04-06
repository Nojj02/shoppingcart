using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventRouterTest
    {
        [Fact]
        public void Handle()
        {
            var api = new TestApiHelper();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<TestResourceEvent>
                {
                    new TestResourceEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    }
                });

            var eventMonitor = new EventMonitor(api);
            
            var handler = new OnAnyEventRecordInListEventHandler<TestResourceEvent>();
            eventMonitor.Subscribe<TestResourceEvent>("resource", handler);

            eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);
        }
    }

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
            var subscription = _eventSubscriptions.First();

            var newEvents = _apiHelper.GetNewEvents(subscription.ResourceName);
            subscription.Handler.Handle(newEvents);
        }
    }

    public interface IApiHelper
    {
        IList<object> GetNewEvents(string resourceName);
    }

    public class TestApiHelper : IApiHelper
    {
        private readonly Dictionary<string, IList<object>> _newApiEvents = new Dictionary<string, IList<object>>();

        public void SetupTestResource<TEvent>(string resourceName, List<TEvent> newTestEvents)
        {
            _newApiEvents.Add(resourceName, newTestEvents.Cast<object>().ToList());
        }

        public IList<object> GetNewEvents(string resourceName)
        {
            return _newApiEvents[resourceName];
        }
    }

    public class TestResourceEvent
    {
        public Guid Id { get; set; }
    }

    public interface IEventHandler
    {
        void Handle(IList<object> newEvents);
    }

    public class OnAnyEventRecordInListEventHandler<T> : IEventHandler
         where T : class
    {
        private readonly List<T> _events = new List<T>();

        public IReadOnlyList<T> Events => _events;

        public void Handle(IList<object> newEvents)
        {
            var matchingEvents =
                newEvents.Select(x => x as T)
                    .Where(x => x != null)
                    .ToList();

            _events.AddRange(matchingEvents);
        }
    }
}
