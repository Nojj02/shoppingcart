using System.Threading.Tasks;

namespace ShoppingCartHandlers.DataAccess
{
    public interface IEventTrackingRepository
    {
        Task<int> GetLastMessageNumber(string resourceName);
        Task UpdateLastMessageNumberAsync(string resourceName, int newLastMessageNumber);
    }
}