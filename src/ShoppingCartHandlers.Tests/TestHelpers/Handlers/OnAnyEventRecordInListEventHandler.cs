using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartHandlers.Handlers;

namespace ShoppingCartHandlers.Tests.TestHelpers.Handlers
{
    public class OnAnyEventRecordInListEventHandler<T> : IEventHandler
        where T : class
    {
        private readonly List<T> _events = new List<T>();

        public IReadOnlyList<T> Events => _events;

        public Task Handle(IList<object> newEvents)
        {
            var matchingEvents =
                newEvents.Select(x => x as T)
                    .Where(x => x != null)
                    .ToList();

            _events.AddRange(matchingEvents);
            return Task.CompletedTask;
        }
    }
}