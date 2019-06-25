using System.Collections.Generic;

namespace ShoppingCartApi.Events
{
    public class TransportMessage
    {
        public string MessageType;
        public IEnumerable<EventInfo> Events = new List<EventInfo>();

    }
}