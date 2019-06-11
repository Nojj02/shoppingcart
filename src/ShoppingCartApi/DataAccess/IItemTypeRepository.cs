using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemTypeRepository : IRepository<ItemType, IItemTypeEvent>
    {
    }
}