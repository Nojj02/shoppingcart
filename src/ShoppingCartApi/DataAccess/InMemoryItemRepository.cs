using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemRepository : InMemoryRepository<Item>, IItemRepository
    {
        public Task<Item> GetAsync(string code)
        {
            var result = Entities.SingleOrDefault(x => x.Code == code);
            return Task.FromResult(result);
        }
    }
}