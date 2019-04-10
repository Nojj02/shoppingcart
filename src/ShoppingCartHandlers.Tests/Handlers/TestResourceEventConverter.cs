using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class TestResourceEventConverter : EventConverter
    {
        public static readonly TestResourceEventConverter Instance = new TestResourceEventConverter();
        
        public TestResourceEventConverter()
            : base(new Dictionary<string, Type>
            {
                { "resource-created", typeof(TestResourceCreatedEvent) },
                { "resource-updated", typeof(TestResourceUpdatedEvent) }
            })
        {
        }
    }
}