using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
        Task SaveAsync(T entity, DateTimeOffset timestamp);
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids);
        Task UpdateAsync(T entity, DateTimeOffset timestamp);
    }

    public interface IRepository<T, TEvent> : IRepository<T>
        where T : IAggregateRoot<TEvent>
        where TEvent : IEvent
    {
        Task<IReadOnlyList<TEvent>> GetEventsAsync(int startIndex, int endIndex);
    }
}