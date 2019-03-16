using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemRepository : IItemRepository
    {
        private readonly List<Item> _items = new List<Item>();

        public Task Save(Item item)
        {
            _items.Add(item);
            return Task.CompletedTask;
        }

        public Task<Item> Get(string code)
        {
            var result = _items.SingleOrDefault(x => x.Code == code);
            return Task.FromResult(result);
        }
    }
}