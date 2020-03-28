using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartHandlers.Tests.TestHelpers;
using ShoppingCartHandlers.Tests.TestHelpers.Handlers;
using ShoppingCartHandlers.Tests.TestHelpers.Web;
using Xunit;

namespace ShoppingCartHandlers.Tests
{
    public class EventMonitorTest
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

            var eventTrackingRepository = new ListEventTrackingRepository();
            var eventMonitor = new EventMonitor(api, eventTrackingRepository);
            
            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(0, lastMessageNumber.Value);
        }
        
        [Fact]
        public async Task TakeAllEvents_ThereIsNoTrackingForLastEventNumberYet()
        {
            var api = new TestEventApi();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<TestResourceCreatedEvent>
                {
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    },
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("C471D99B-2C72-44F6-898F-F0BABCBAC9D7")
                    },
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("6843FE44-3029-47D3-A9B9-C21A3BAB4397")
                    }
                });

            var eventTrackingRepository = new ListEventTrackingRepository(new List<EventTracking>
            {
                EventTracking.Preset("resource", MessageNumber.New(1))
            });

            var eventMonitor = new EventMonitor(api, eventTrackingRepository);

            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("6843FE44-3029-47D3-A9B9-C21A3BAB4397"), handler.Events[0].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(2, lastMessageNumber.Value);
        }
        
        [Fact]
        public async Task TakeEventsOnlyAfterLastMessageNumber()
        {
            var api = new TestEventApi();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<TestResourceCreatedEvent>
                {
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    },
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("C471D99B-2C72-44F6-898F-F0BABCBAC9D7")
                    },
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("6843FE44-3029-47D3-A9B9-C21A3BAB4397")
                    }
                });


            var eventTrackingRepository = new ListEventTrackingRepository();
            var eventMonitor = new EventMonitor(api, eventTrackingRepository);

            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            await eventMonitor.Poll();

            Assert.Equal(3, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);
            Assert.Equal(new Guid("C471D99B-2C72-44F6-898F-F0BABCBAC9D7"), handler.Events[1].Id);
            Assert.Equal(new Guid("6843FE44-3029-47D3-A9B9-C21A3BAB4397"), handler.Events[2].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(2, lastMessageNumber.Value);
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


            var eventTrackingRepository = new ListEventTrackingRepository();
            var eventMonitor = new EventMonitor(api, eventTrackingRepository);

            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            var alternativeHandler = new OnAnyEventRecordInListEventHandler<AlternativeTestResourceEvent>();
            eventMonitor.Subscribe<AlternativeTestResourceEvent>("alternativeResource", alternativeHandler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);

            Assert.Equal(1, alternativeHandler.Events.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), alternativeHandler.Events[0].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(0, lastMessageNumber.Value);
            var alternativeLastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("alternativeResource");
            Assert.Equal(0, alternativeLastMessageNumber.Value);
        }

        [Fact]
        public async Task MultipleHandlersForTheSameResource()
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
            
            var eventTrackingRepository = new ListEventTrackingRepository();
            var eventMonitor = new EventMonitor(api, eventTrackingRepository);

            var handler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);

            var alternativeHandler = new OnAnyEventRecordInListEventHandler<TestResourceCreatedEvent>();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", alternativeHandler);

            await eventMonitor.Poll();

            Assert.Equal(1, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);

            Assert.Equal(1, alternativeHandler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), alternativeHandler.Events[0].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(0, lastMessageNumber.Value);
        }

        [Fact]
        public async Task MultipleEventsOfTheSameResourceForTheSameHandler()
        {
            var api = new TestEventApi();
            api.SetupTestResource(
                resourceName: "resource",
                newTestEvents: new List<ITestResourceCreatedEvent>
                {
                    new TestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    },
                    new AnotherTestResourceCreatedEvent
                    {
                        Id = new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0")
                    }
                });

            var eventTrackingRepository = new ListEventTrackingRepository();
            var eventMonitor = new EventMonitor(api, eventTrackingRepository);

            var handler = new OnAnyEventRecordInListEventHandler();
            eventMonitor.Subscribe<TestResourceCreatedEvent>("resource", handler);
            eventMonitor.Subscribe<AnotherTestResourceCreatedEvent>("resource", handler);

            await eventMonitor.Poll();

            Assert.Equal(2, handler.Events.Count);
            Assert.Equal(new Guid("0683f052-40f0-4bff-879e-f4bea94c0ed0"), handler.Events[0].Id);

            var lastMessageNumber = await eventTrackingRepository.GetLastMessageNumber("resource");
            Assert.Equal(1, lastMessageNumber.Value);
        }
    }
}
