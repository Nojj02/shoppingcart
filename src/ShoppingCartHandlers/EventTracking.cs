namespace ShoppingCartHandlers
{
    public class EventTracking
    {
        public EventTracking(string resourceName, int lastMessageNumber)
        {
            ResourceName = resourceName;
            LastMessageNumber = lastMessageNumber;
        }

        public string ResourceName { get; private set; }

        /// <summary>
        /// 0-based tracking of the number associated with an Event.
        /// </summary>
        public int LastMessageNumber { get; private set; }
    }
}