using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

namespace ShoppingCartApi.DataAccess
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : IAggregateRoot
    {
        public List<T> Entities { get; } = new List<T>();

        public event Action<T> EventOccurred = x => { };

        public Task SaveAsync(T entity, DateTimeOffset timestamp)
        {
            Entities.Add(entity);

            EventOccurred(entity);
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
            if (matchingT != null)
            {
                Entities.Remove(matchingT);
            }

            Entities.Add(entity);
            
            EventOccurred(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity, DateTimeOffset dateTimeOffset)
        {
            var matchingT = Entities.SingleOrDefault(x => x.Id == entity.Id);
            if (matchingT != null)
            {
                Entities.Remove(matchingT);
            }

            Entities.Add(entity);
            
            EventOccurred(entity);
            return Task.CompletedTask;
        }
    }

    public class InMemoryRepository<T, TEvent> : IRepository<T, TEvent> 
        where T : IAggregateRoot<TEvent> 
        where TEvent : IEvent
    {
        private readonly Func<Guid, IReadOnlyList<TEvent>, T> _reconstitute;
        
        public List<TEvent> Events { get; } = new List<TEvent>();

        public InMemoryRepository(Func<Guid, IReadOnlyList<TEvent>, T> reconstitute)
        {
            _reconstitute = reconstitute;
        }

        public event Action<T> EventOccurred = x => { };

        public Task SaveAsync(T entity, DateTimeOffset timestamp)
        {
            Events.AddRange(entity.Events);

            EventOccurred(entity);
            return Task.CompletedTask;
        }

        public Task<T> GetAsync(Guid id)
        {
            var matchingEvents = Events
                .Where(x => x.Id == id)
                .ToList();

            var result = _reconstitute(id, matchingEvents);
            
            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids)
        {
            var matchingEventsGroupings = Events
                .Where(x => ids.Contains(x.Id))
                .GroupBy(x => x.Id)
                .ToList();

            var result = 
                matchingEventsGroupings
                    .Select(x => _reconstitute(x.Key, x.ToList()))
                    .ToList();
            
            return Task.FromResult((IReadOnlyList<T>)result);
        }

        public Task UpdateAsync(T entity, DateTimeOffset dateTimeOffset)
        {
            Events.AddRange(entity.NewEvents);

            EventOccurred(entity);
            return Task.CompletedTask;
        }
        
        public Task<IReadOnlyList<TEvent>> GetEventsAsync(int startIndex, int endIndex)
        {
            return Task.FromResult((IReadOnlyList<TEvent>)Events);
        }
    }
}