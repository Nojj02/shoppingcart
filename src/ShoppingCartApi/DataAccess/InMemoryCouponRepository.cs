using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryCouponRepository : InMemoryRepository<Coupon>, ICouponRepository
    {
        public Task<Coupon> GetByAsync(string code)
        {
            var entity = Entities.SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
