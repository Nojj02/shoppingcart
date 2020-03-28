using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Web
{
    public interface IEventApi
    {
        Task<IList<object>> GetEventsAfterAsync(string resourceName, MessageNumber lastMessageNumber);
    }
}