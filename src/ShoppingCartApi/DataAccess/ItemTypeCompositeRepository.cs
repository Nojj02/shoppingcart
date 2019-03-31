using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemTypeCompositeRepository : 
        CompositeRepository<ItemType, ItemTypeRepository, ItemTypeReadRepository>,
        IItemTypeRepository
    {
        public ItemTypeCompositeRepository(string connectionString)
             : base(new ItemTypeRepository(connectionString), new ItemTypeReadRepository(connectionString))
        {
        }


        public async Task<ItemType> GetAsync(string code)
        {
            return await ReadRepository.GetAsync(code);
        }
    }
}