using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
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