using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingCartHandlers.Handlers;

namespace ShoppingCartHandlers
{
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