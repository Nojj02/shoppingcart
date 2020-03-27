using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartHandlers.DataAccess;

namespace ShoppingCartHandlers.Tests.TestHelpers.DataAccess
{
    public class TestEventTrackingRepository : IEventTrackingRepository
    {
        private readonly IReadOnlyList<EventTracking> _eventTrackings;

        public TestEventTrackingRepository()
            : this(new List<EventTracking>())
        {
        }

        public TestEventTrackingRepository(IReadOnlyList<EventTracking> eventTrackings)
        {
            _eventTrackings = eventTrackings;
        }

        public int GetLastMessageNumber(string resourceName)
        {
            var eventTracking = _eventTrackings.SingleOrDefault(x => x.ResourceName == resourceName);
            return eventTracking?.LastMessageNumber ?? -1;
        }

        public Task UpdateLastMessageNumberAsync(string resourceName, int lastMessageNumber)
        {
            _eventTrackings.Single(x => x.ResourceName == resourceName).UpdateLastMessageNumber(lastMessageNumber);
            return Task.CompletedTask;
        }
    }
}