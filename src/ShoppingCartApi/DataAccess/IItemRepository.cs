using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemRepository : IRepository<Item, IItemEvent>
    {
    }
}