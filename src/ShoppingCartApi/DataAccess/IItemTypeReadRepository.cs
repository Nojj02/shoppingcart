using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemTypeReadRepository
    {
        Task<ItemTypeReadModel> GetAsync(string code);
    }
}