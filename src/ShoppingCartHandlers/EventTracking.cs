using System;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace ShoppingCartHandlers
{
    public class EventTracking
    {
        public static EventTracking New(string resourceName)
        {
            return new EventTracking
            {
                ResourceName = resourceName,
                LastMessageNumber = MessageNumber.NotSet
            };
        }

        public static EventTracking Preset(string resourceName, MessageNumber messageNumber)
        {
            return new EventTracking
            {
                ResourceName = resourceName,
                LastMessageNumber = messageNumber
            };
        }

        private EventTracking()
        {
        }

        public string ResourceName { get; private set; }

        /// <summary>
        /// 0-based tracking of the number associated with an Event.
        /// </summary>
        public MessageNumber LastMessageNumber { get; private set; }

        public void UpdateLastMessageNumber(MessageNumber newLastMessageNumber)
        {
            if (newLastMessageNumber < LastMessageNumber)
            {
                throw new LastMessageNumberMustNeverDecreaseException(LastMessageNumber, newLastMessageNumber);
            }

            LastMessageNumber = newLastMessageNumber;
        }

        public class LastMessageNumberMustNeverDecreaseException : Exception
        {
            public LastMessageNumberMustNeverDecreaseException(MessageNumber currentValue, MessageNumber attemptedValue)
                : base($"Attempted to set the last message number to [{attemptedValue}] when the current last message number is [{currentValue}]. The value is expected to always increase.")
            {

            }
        }
    }
}