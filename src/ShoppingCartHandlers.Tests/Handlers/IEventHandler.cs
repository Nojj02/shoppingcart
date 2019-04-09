using System.Collections.Generic;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public interface IEventHandler
    {
        void Handle(IList<object> newEvents);
    }
}