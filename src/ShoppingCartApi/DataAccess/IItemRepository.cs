using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetAsync(string code);
    }
}