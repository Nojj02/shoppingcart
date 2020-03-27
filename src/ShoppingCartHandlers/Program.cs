using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingCartHandlers.DataAccess;
using ShoppingCartHandlers.Handlers;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Service started...");
            var httpClientWrapper = new HttpClientWrapper();
            var eventConverter = new EventConverter(new Dictionary<string, Type>
            {
                //{ "coupon", typeof(CouponDto) },
                { "itemtype-created", typeof(ItemTypeCreatedEvent) }
            });
            var eventApi = new EventApi(
                host: "http://localhost:9050",
                httpClientWrapper: httpClientWrapper,
                eventConverter: eventConverter,
                batchSize: 10
            );

            var eventMonitor = new EventMonitor(eventApi:eventApi, eventTrackingRepository: new ListEventTrackingRepository());
            eventMonitor.Subscribe<ItemTypeCreatedEvent>("itemType", new ConsoleEventHandler());
            
            await PollEvents(eventMonitor);

            var timer = new Timer(async state =>
                {
                    Console.WriteLine("Polling...");
                    await PollEvents(eventMonitor);
                }, 
                state: null,
                period: TimeSpan.FromSeconds(5), 
                dueTime: TimeSpan.FromSeconds(0));
            

            Console.WriteLine("Press any key to quit...");
            while (!Console.KeyAvailable)
            {
                await Task.Delay(1000);
            }
            
            Console.ReadKey(true);
        }

        private static async Task PollEvents(EventMonitor eventMonitor)
        {
            try
            {
                await eventMonitor.Poll();
            }
            catch (EventApiRequestFailedException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class ListEventTrackingRepository : IEventTrackingRepository
    {
        private readonly List<EventTracking> _eventTrackingList;

        public ListEventTrackingRepository(List<EventTracking> predefinedEventTrackingList = null)
        {
            _eventTrackingList = predefinedEventTrackingList ?? new List<EventTracking>();
        }

        public int GetLastMessageNumber(string resourceName)
        {
            var eventTracking = _eventTrackingList.SingleOrDefault(x => x.ResourceName == resourceName);
            if (eventTracking == null)
            {
                eventTracking = new EventTracking(resourceName, -1);
                _eventTrackingList.Add(eventTracking);
            }

            return eventTracking.LastMessageNumber;
        }

        public Task UpdateLastMessageNumberAsync(string resourceName, int lastMessageNumber)
        {
            _eventTrackingList.Single(x => x.ResourceName == resourceName).UpdateLastMessageNumber(lastMessageNumber);
            return Task.CompletedTask;
        }
    }

    public class ConsoleEventHandler : IEventHandler
    {
        public Task Handle(IList<object> newEvents)
        {
            Console.WriteLine("Events received.");
            Console.WriteLine("=> " + JsonConvert.SerializeObject(newEvents));
            return Task.CompletedTask;
        }
    }
}
