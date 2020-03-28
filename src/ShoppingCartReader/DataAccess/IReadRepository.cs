using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface IReadRepository<TReadModel> where TReadModel : class, IEntity
    {
        Task<TReadModel> GetAsync(Guid id);
        Task<IReadOnlyList<TReadModel>> GetAsync(IEnumerable<Guid> ids);
        Task SaveAsync(TReadModel entity);
        Task UpdateAsync(TReadModel entity);
    }
}