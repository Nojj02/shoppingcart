using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeRepository : IItemTypeRepository
    {
        private readonly List<ItemType> _itemTypes = new List<ItemType>();

        public Task SaveAsync(ItemType itemType)
        {
            _itemTypes.Add(itemType);
            return Task.CompletedTask;
        }

        public Task<ItemType> GetAsync(Guid id)
        {
            var entity = _itemTypes.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(entity);
        }

        public Task<ItemType> GetByCodeAsync(string code)
        {
            var entity = _itemTypes.SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
