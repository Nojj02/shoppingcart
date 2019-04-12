using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public interface IEventApi
    {
        Task<IList<object>> GetAllEventsAsync(string resourceName);
        Task<IList<object>> GetEventsAfterAsync(string resourceName, int lastMessageNumber);
    }
}