using System.Threading.Tasks;

namespace ShoppingCartHandlers.DataAccess
{
    public interface IEventTrackingRepository
    {
        int GetLastMessageNumber(string resourceName);
        Task UpdateLastMessageNumberAsync(string resourceName, int lastMessageNumber);
    }
}