using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public interface IEventApi
    {
        Task<IList<object>> GetNewEventsAsync(string resourceName);
    }
}