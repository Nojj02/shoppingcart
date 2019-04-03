using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemReadRepository : InMemoryRepository<ItemReadModel>, IItemReadRepository
    {
        public Task<ItemReadModel> GetAsync(string code)
        {
            var result = Entities
                .SingleOrDefault(x => x.Code == code);
            return Task.FromResult(result);
        }
    }
}