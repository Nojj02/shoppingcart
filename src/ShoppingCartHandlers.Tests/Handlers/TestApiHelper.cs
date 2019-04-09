using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class TestApiHelper : IApiHelper
    {
        private readonly Dictionary<string, IList<object>> _newApiEvents = new Dictionary<string, IList<object>>();

        public void SetupTestResource<TEvent>(string resourceName, List<TEvent> newTestEvents)
        {
            _newApiEvents.Add(resourceName, newTestEvents.Cast<object>().ToList());
        }

        public IList<object> GetNewEvents(string resourceName)
        {
            return _newApiEvents.GetValueOrDefault(resourceName) ?? new List<object>();
        }
    }
}