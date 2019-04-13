using System;
using System.Collections.Generic;

namespace ShoppingCartHandlers
{
    public class EventConverter
    {
        public static EventConverter Empty => new EventConverter(new Dictionary<string, Type>());

        private readonly Dictionary<string, Type> _knownTypes;

        public EventConverter(Dictionary<string, Type> knownTypes)
        {
            _knownTypes = knownTypes;
        }

        public Type GetTypeOf(string typeName)
        {
            return _knownTypes[typeName];
        }
    }
}