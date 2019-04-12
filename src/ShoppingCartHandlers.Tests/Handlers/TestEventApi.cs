using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class TestEventApi : IEventApi
    {
        private readonly Dictionary<string, IList<object>> _newApiEvents = new Dictionary<string, IList<object>>();

        public void SetupTestResource<TEvent>(string resourceName, List<TEvent> newTestEvents)
        {
            _newApiEvents.Add(resourceName, newTestEvents.Cast<object>().ToList());
        }

        public Task<IList<object>> GetAllEventsAsync(string resourceName)
        {
            return GetEventsAfterAsync(resourceName, -1);
        }

        public Task<IList<object>> GetEventsAfterAsync(string resourceName, int lastMessageNumber)
        {
            return Task.FromResult(_newApiEvents.GetValueOrDefault(resourceName) ?? new List<object>());
        }
    }
}