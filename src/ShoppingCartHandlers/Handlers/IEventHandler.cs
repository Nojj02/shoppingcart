using System.Collections.Generic;

namespace ShoppingCartHandlers.Handlers
{
    public interface IEventHandler
    {
        void Handle(IList<object> newEvents);
    }
}