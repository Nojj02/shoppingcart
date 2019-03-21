using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface ICouponRepository
    {
        Task SaveAsync(Coupon coupon);

        Task<Coupon> GetAsync(Guid id);

        Task<Coupon> GetByCouponCodeAsync(string code);
    }
}