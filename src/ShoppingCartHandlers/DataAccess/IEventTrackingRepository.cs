namespace ShoppingCartHandlers.DataAccess
{
    public interface IEventTrackingRepository
    {
        int GetLastMessageNumber(string resourceName);
    }
}