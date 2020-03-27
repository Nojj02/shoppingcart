using System.Linq;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public class InMemoryCouponReadRepository : InMemoryRepository<CouponReadModel>, ICouponReadRepository
    {
        public Task<CouponReadModel> GetAsync(string code)
        {
            var entity = Entities
                .SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}