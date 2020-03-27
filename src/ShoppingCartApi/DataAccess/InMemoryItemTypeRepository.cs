using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeRepository : InMemoryRepository<ItemType, IItemTypeEvent>, IItemTypeRepository
    {
        public InMemoryItemTypeRepository() 
            : base((id, x) => new ItemType(id, x))
        {
        }
    }
}
