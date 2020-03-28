using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public interface ICouponRepository : IRepository<Coupon, ICouponEvent>
    {
    }
}