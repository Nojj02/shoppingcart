using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartHandlers.Tests.Handlers
{
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