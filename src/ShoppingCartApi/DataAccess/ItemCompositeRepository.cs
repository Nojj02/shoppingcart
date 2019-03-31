using System.Threading.Tasks;
using NuGet.Frameworks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemCompositeRepository : 
        CompositeRepository<Item, ItemRepository, ItemReadRepository>, 
        IItemRepository
    {
        public ItemCompositeRepository(string connectionString)
            : base(new ItemRepository(connectionString), new ItemReadRepository(connectionString))
        {
        }

        public async Task<Item> GetAsync(string code)
        {
            return await ReadRepository.GetAsync(code);
        }
    }
}