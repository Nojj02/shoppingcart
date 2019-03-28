using System;

namespace ShoppingCartApi.Model.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}