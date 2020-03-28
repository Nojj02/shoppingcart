using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryCouponRepository : InMemoryRepository<Coupon, ICouponEvent>, ICouponRepository
    {
        public InMemoryCouponRepository()
            : base((id, x) => new Coupon(id, x))
        {
        }
    }
}
