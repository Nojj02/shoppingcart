using System;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface IItemTypeReadRepository
    {
        Task<ItemTypeReadModel> GetAsync(Guid id);

        Task<ItemTypeReadModel> GetAsync(string code);
    }
}