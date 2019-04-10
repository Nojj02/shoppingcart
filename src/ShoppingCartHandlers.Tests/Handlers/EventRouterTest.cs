using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventRouterTests
    {
        [Fact]
        public async Task HandleSingleSubscription()
        {
            var api = new TestEventApi();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<TestResourceCreatedEvent>
                {
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    }
                });

            var eventMonitor = new EventMonitor(api);
            
            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);
        }

        [Fact]
        public async Task HandleMultipleSubscriptions()
        {
            var api = new TestEventApi();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<TestResourceCreatedEvent>
                {
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    }
                });
            api.SetupTestResource(
                resourceName: "alternativeResource",
                newTestEvents: new List<AlternativeTestResourceEvent>
                {
                    new AlternativeTestResourceEvent
                    {
                        Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                    }
                });

            var eventMonitor = new EventMonitor(api);

            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            var alternativeHandler = new OnAnyEventRecordInListEventHandler<AlternativeTestResourceEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("alternativeResource", alternativeHandler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);

            Assert.Equal(1, alternativeHandler.Events.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), alternativeHandler.Events[0].Id);
        }
    }
}
