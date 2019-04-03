using System;
using System.Collections.Generic;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemTypeRepository : InMemoryRepository<ItemType>, IItemTypeRepository
    {
    }
}
