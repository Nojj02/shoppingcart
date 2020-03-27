using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface ICouponReadRepository
    {
        Task<CouponReadModel> GetAsync(string code);
    }
}