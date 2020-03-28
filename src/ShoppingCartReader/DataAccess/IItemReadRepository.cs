using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface IItemReadRepository : IReadRepository<ItemReadModel>
    {
        Task<ItemReadModel> GetAsync(string code);
    }
}