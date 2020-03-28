using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartHandlers.DataAccess;

namespace ShoppingCartHandlers
{
    public class ListEventTrackingRepository : IEventTrackingRepository
    {
        private readonly List<EventTracking> _eventTrackingList;

        public ListEventTrackingRepository(List<EventTracking> predefinedEventTrackingList = null)
        {
            _eventTrackingList = predefinedEventTrackingList ?? new List<EventTracking>();
        }

        public Task<int> GetLastMessageNumber(string resourceName)
        {
            var eventTracking = _eventTrackingList.SingleOrDefault(x => x.ResourceName == resourceName);
            if (eventTracking == null)
            {
                eventTracking = new EventTracking(resourceName, -1);
                _eventTrackingList.Add(eventTracking);
            }

            return Task.FromResult(eventTracking.LastMessageNumber);
        }

        public Task UpdateLastMessageNumberAsync(string resourceName, int newLastMessageNumber)
        {
            _eventTrackingList.Single(x => x.ResourceName == resourceName).UpdateLastMessageNumber(newLastMessageNumber);
            return Task.CompletedTask;
        }
    }
}