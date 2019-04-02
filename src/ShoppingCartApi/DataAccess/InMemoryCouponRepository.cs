using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryCouponRepository : InMemoryRepository<Coupon>, ICouponRepository, ICouponReadRepository
    {
        public Task<CouponReadModel> GetAsync(string code)
        {
            var entity = Entities
                .Select(CouponReadModel.Map).SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
