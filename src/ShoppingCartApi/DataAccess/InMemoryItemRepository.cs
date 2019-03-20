using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemRepository : IItemRepository
    {
        private readonly List<Item> _items = new List<Item>();

        public Task SaveAsync(Item item)
        {
            _items.Add(item);
            return Task.CompletedTask;
        }

        public Task<Item> GetAsync(string code)
        {
            var result = _items.SingleOrDefault(x => x.Code == code);
            return Task.FromResult(result);
        }

        public Task<Item> GetAsync(Guid id)
        {
            var result = _items.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<Item>> GetAsync(IEnumerable<Guid> ids)
        {
            var result = _items.Where(x => ids.Contains(x.Id)).ToList() as IReadOnlyList<Item>;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(Item entity)
        {
            var matchingItem = _items.SingleOrDefault(x => x.Code == entity.Code);
            _items.Remove(matchingItem);
            _items.Add(entity);
            return Task.CompletedTask;
        }
    }
}