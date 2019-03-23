using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeRepository : InMemoryRepository<ItemType>, IItemTypeRepository
    {
        public Task<ItemType> GetAsync(string code)
        {
            var entity = Entities.SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
