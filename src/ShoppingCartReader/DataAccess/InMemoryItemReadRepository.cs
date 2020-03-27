using System.Linq;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
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