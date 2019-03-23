using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<ItemType> GetAsync(string code);
    }
}