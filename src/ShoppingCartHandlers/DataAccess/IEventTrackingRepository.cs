using System.Threading.Tasks;

namespace ShoppingCartHandlers.DataAccess
{
    public interface IEventTrackingRepository
    {
        Task<MessageNumber> GetLastMessageNumber(string resourceName);
        Task UpdateLastMessageNumberAsync(string resourceName, MessageNumber newLastMessageNumber);
    }
}