using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeReadRepository : InMemoryRepository<ItemTypeReadModel>, IItemTypeReadRepository
    {
        public Task<ItemTypeReadModel> GetAsync(string code)
        {
            var entity = Entities
                .SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}