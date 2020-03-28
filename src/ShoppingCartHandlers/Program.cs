using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShoppingCartHandlers.DataAccess.TypeHandlers;
using ShoppingCartHandlers.Handlers;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Service started...");
            Dapper.SqlMapper.AddTypeHandler(typeof(MessageNumber), new MessageNumberEventHandler());

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

            var eventMonitor = new EventMonitor(eventApi:eventApi, eventTrackingRepository: new EventTrackingRepository());
            eventMonitor.Subscribe<ItemTypeCreatedEvent>("itemType", new ItemTypeReadModelHandler());
            eventMonitor.Subscribe<ItemTypeCreatedEvent>("itemType", new ConsoleEventHandler());

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            var task = Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine("Polling...");
                    await PollEvents(eventMonitor);
                    await Task.Delay(1000, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }
            }, cancellationTokenSource.Token);

            Console.WriteLine("Press any key to quit...");

            while (!Console.KeyAvailable)
            {
                await Task.Delay(1000);
            }

            Console.ReadKey(true);
            cancellationTokenSource.Cancel();
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
