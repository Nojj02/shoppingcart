using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryCouponRepository : ICouponRepository
    {
        private readonly List<Coupon> _coupons = new List<Coupon>();

        public Task SaveAsync(Coupon coupon)
        {
            _coupons.Add(coupon);
            return Task.CompletedTask;
        }

        public Task<Coupon> GetAsync(Guid id)
        {
            var entity = _coupons.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(entity);
        }

        public Task<Coupon> GetByCouponCodeAsync(string code)
        {
            var entity = _coupons.SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
