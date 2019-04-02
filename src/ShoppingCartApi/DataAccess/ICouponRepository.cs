using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface ICouponRepository : IRepository<Coupon>
    {
    }
}