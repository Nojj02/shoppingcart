using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IRepository<T>
        where T : AggregateRoot
    {
        Task SaveAsync(T entity);
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids);
        Task UpdateAsync(T entity);
    }
}