using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public interface IRepository<T>
        where T : IEntity
    {
        Task SaveAsync(T entity);
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids);
        Task UpdateAsync(T entity);
    }
}