using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemRepository
    {
        Task Save(Item item);
        Task<Item> Get(string code);
    }
}