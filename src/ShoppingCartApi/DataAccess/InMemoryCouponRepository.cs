using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryCouponRepository : InMemoryRepository<Coupon>, ICouponRepository
    {
    }
}
