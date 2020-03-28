using System;
using System.Collections.Generic;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryItemRepository : InMemoryRepository<Item, IItemEvent>, IItemRepository
    {
        public InMemoryItemRepository()
            : base((id, x) => new Item(id, x))
        {
        }
    }
}