using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShoppingCartHandlers.DataAccess;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var httpClientWrapper = new HttpClientWrapper();
            var eventConverter = new EventConverter(new Dictionary<string, Type>
            {
                { "", typeof(object) }
            });
            var eventApi = new EventApi(
                host: "http://localhost:9050",
                httpClientWrapper: httpClientWrapper,
                eventConverter: eventConverter,
                batchSize: 10
            );

            var eventMonitor = new EventMonitor(eventApi:eventApi, eventTrackingRepository: (IEventTrackingRepository)null);
            await eventMonitor.Poll();

            var timer = new Timer(async state =>
                    {
                        await eventMonitor.Poll();
                    }, 
                state: null,
                period: TimeSpan.FromSeconds(5000), 
                dueTime: TimeSpan.FromSeconds(0));

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();
        }
    }
}
