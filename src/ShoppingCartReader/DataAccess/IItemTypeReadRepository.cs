using System;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface IItemTypeReadRepository : IReadRepository<ItemTypeReadModel>
    {
        Task<ItemTypeReadModel> GetAsync(string code);
    }
}