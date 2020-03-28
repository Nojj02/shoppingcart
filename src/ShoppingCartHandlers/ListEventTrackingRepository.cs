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

        public Task<MessageNumber> GetLastMessageNumber(string resourceName)
        {
            var eventTracking = _eventTrackingList.SingleOrDefault(x => x.ResourceName == resourceName);
            if (eventTracking == null)
            {
                eventTracking = EventTracking.New(resourceName);
                _eventTrackingList.Add(eventTracking);
            }

            return Task.FromResult(eventTracking.LastMessageNumber);
        }

        public Task UpdateLastMessageNumberAsync(string resourceName, MessageNumber newLastMessageNumber)
        {
            _eventTrackingList.Single(x => x.ResourceName == resourceName).UpdateLastMessageNumber(newLastMessageNumber);
            return Task.CompletedTask;
        }
    }
}