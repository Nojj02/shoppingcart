using System;
using System.Collections.Generic;
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
                { "coupons", typeof(CouponDto) }
            });
            var eventApi = new EventApi(
                host: "http://localhost:9050",
                httpClientWrapper: httpClientWrapper,
                eventConverter: eventConverter,
                batchSize: 10
            );

            var eventMonitor = new EventMonitor(eventApi:eventApi, eventTrackingRepository: new ListEventTrackingRepository());
            eventMonitor.Subscribe<CouponDto>("coupons", new ConsoleEventHandler());
            await eventMonitor.Poll();

            var timer = new Timer(async state =>
                    {
                        Console.WriteLine("Polling...");
                        await eventMonitor.Poll();
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
    }

    public class ListEventTrackingRepository : IEventTrackingRepository
    {
        public int GetLastMessageNumber(string resourceName)
        {
            return -1;
        }
    }

    public class ConsoleEventHandler : IEventHandler
    {
        public void Handle(IList<object> newEvents)
        {
            Console.WriteLine("Events received.");
            Console.WriteLine("=> " + JsonConvert.SerializeObject(newEvents));
        }
    }

    public class CouponDto
    {
        public string Code { get; set; }
        public double PercentOff { get; set; }
        public Guid Id { get; set; }
    }
}
