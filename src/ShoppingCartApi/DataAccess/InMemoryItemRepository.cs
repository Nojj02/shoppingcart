using System;
using System.Collections.Generic;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemRepository : InMemoryRepository<Item>, IItemRepository
    {
    }
}