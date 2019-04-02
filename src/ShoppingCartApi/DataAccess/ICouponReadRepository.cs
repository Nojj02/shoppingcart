using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface ICouponReadRepository
    {
        Task<CouponReadModel> GetAsync(string code);
    }
}