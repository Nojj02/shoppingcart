using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemRepository
    {
        Task SaveAsync(Item item);

        Task<Item> GetAsync(string code);

        Task<IReadOnlyList<Item>> GetAsync(IEnumerable<string> code);

        Task UpdateAsync(Item entity);
    }
}