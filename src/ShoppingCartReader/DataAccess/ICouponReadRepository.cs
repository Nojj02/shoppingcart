using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface ICouponReadRepository : IReadRepository<CouponReadModel>
    {
        Task<CouponReadModel> GetAsync(string code);
    }
}