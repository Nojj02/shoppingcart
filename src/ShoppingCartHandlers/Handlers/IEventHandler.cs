using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Handlers
{
    public interface IEventHandler
    {
        Task Handle(IList<object> newEvents);
    }
}