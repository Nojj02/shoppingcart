using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : AggregateRoot
    {
        public List<T> Entities { get; } = new List<T>();

        public Task SaveAsync(T entity)
        {
            Entities.Add(entity);
            return Task.CompletedTask;
        }

        public Task<T> GetAsync(Guid id)
        {
            var result = Entities.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids)
        {
            var result = Entities.Where(x => ids.Contains(x.Id)).ToList() as IReadOnlyList<T>;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(T entity)
        {
            var matchingT = Entities.SingleOrDefault(x => x.Id == entity.Id);
            Entities.Remove(matchingT);
            Entities.Add(entity);
            return Task.CompletedTask;
        }
    }
}