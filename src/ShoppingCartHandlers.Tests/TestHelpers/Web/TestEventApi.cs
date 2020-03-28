using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartHandlers.Web;

namespace ShoppingCartHandlers.Tests.TestHelpers.Web
{
    public class TestEventApi : IEventApi
    {
        private readonly Dictionary<string, IList<object>> _newApiEvents = new Dictionary<string, IList<object>>();

        public void SetupTestResource<TEvent>(string resourceName, List<TEvent> newTestEvents)
        {
            _newApiEvents.Add(resourceName, newTestEvents.Cast<object>().ToList());
        }

        public Task<IList<object>> GetEventsAfterAsync(string resourceName, MessageNumber lastMessageNumber)
        {
            var events = (_newApiEvents.GetValueOrDefault(resourceName) ?? new List<object>())
                .Skip(lastMessageNumber == MessageNumber.NotSet ? 0 : lastMessageNumber.Value + 1)
                .ToList();
            return Task.FromResult((IList<object>)events);
        }
    }
}