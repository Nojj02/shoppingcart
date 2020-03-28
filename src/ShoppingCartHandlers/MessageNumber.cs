using System;

namespace ShoppingCartHandlers
{
    public class MessageNumber
    {
        public static MessageNumber NotSet = new NotSetMessageNumber();
        private readonly int _value;

        public static MessageNumber New(int value)
        {
            if (value < 0)
            {
                throw new InvalidMessageNumberException("Message numbers should be positive, or use the special default value.");
            }
            return new MessageNumber(value);
        }

        protected MessageNumber(int value)
        {
            _value = value;
        }

        public virtual int Value => _value;

        public virtual MessageNumber AddMessageCount(int newEventsCount)
        {
            return New(Value + newEventsCount);
        }

        public override string ToString()
        {
            return this == NotSet ? "Not Set" : Value.ToString();
        }

        public static bool operator ==(MessageNumber a, MessageNumber b)
        {
            return a._value == b._value;
        }

        public static bool operator !=(MessageNumber a, MessageNumber b)
        {
            return a._value != b._value;
        }

        public static bool operator <(MessageNumber a, MessageNumber b)
        {
            return a._value < b._value;
        }

        public static bool operator >(MessageNumber a, MessageNumber b)
        {
            return a._value > b._value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((MessageNumber)obj);
        }

        protected bool Equals(MessageNumber other)
        {
            return _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value;
        }

        private class NotSetMessageNumber : MessageNumber
        {
            public NotSetMessageNumber()
                : base(-1000)
            {
            }

            public override int Value => throw new UnusableValueException();

            public override MessageNumber AddMessageCount(int newEventsCount)
            {
                return newEventsCount == 0 ? this : new MessageNumber(newEventsCount - 1);
            }

            private class UnusableValueException : Exception
            {
                public UnusableValueException()
                    : base("Do not depend on the numerical value of not set. Add a special handling for this value type.")
                {
                }
            }
        }

        public class InvalidMessageNumberException : Exception
        {
            public InvalidMessageNumberException(string message)
                : base(message)
            {
            }
        }
    }
}