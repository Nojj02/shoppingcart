using System.Collections.Generic;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public interface IApiHelper
    {
        IList<object> GetNewEvents(string resourceName);
    }
}