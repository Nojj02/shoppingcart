using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeRepository : InMemoryRepository<ItemType>, IItemTypeRepository, IItemTypeReadRepository
    {
        public Task<ItemTypeReadModel> GetAsync(string code)
        {
            var entity = Entities
                .Select(ItemTypeReadModel.Map)
                .SingleOrDefault(x => x.Code == code);

            return Task.FromResult(entity);
        }
    }
}
