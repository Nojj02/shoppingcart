using System;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemTypeRepository
    {
        Task SaveAsync(ItemType itemType);

        Task<ItemType> GetAsync(Guid id);

        Task<ItemType> GetByCodeAsync(string code);
    }
}