using System;

namespace ShoppingCartHandlers.Tests.TestHelpers
{
    public class AnotherTestResourceCreatedEvent : ITestResourceCreatedEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}