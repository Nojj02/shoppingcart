using System;

namespace ShoppingCartHandlers.Tests.TestHelpers
{
    public class TestResourceCreatedEvent : ITestResourceCreatedEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}