using System;

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

        public void UpdateLastMessageNumber(int newLastMessageNumber)
        {
            if (newLastMessageNumber < LastMessageNumber)
            {
                throw new LastMessageNumberMustNeverDecreaseException(LastMessageNumber, newLastMessageNumber);
            }

            LastMessageNumber = newLastMessageNumber;
        }

        public class LastMessageNumberMustNeverDecreaseException : Exception
        {
            public LastMessageNumberMustNeverDecreaseException(int currentValue, int attemptedValue)
                : base($"Attempted to set the last message number to [{attemptedValue}] when the current last message number is [{currentValue}]. The value is expected to always increase.")
            {

            }
        }
    }
}