using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class CompositeRepository<TEntity, TRepo, TReadRepo> : IRepository<TEntity>
        where TEntity : AggregateRoot
        where TRepo : IRepository<TEntity>
        where TReadRepo : IRepository<TEntity>
    {
        public CompositeRepository(TRepo repository, TReadRepo readRepository)
        {
            Repository = repository;
            ReadRepository = readRepository;
        }

        protected TRepo Repository { get; }
        protected TReadRepo ReadRepository { get; }

        public async Task SaveAsync(TEntity entity)
        {
            await Repository.SaveAsync(entity);
            await ReadRepository.SaveAsync(entity);
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await ReadRepository.GetAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(IEnumerable<Guid> ids)
        {
            return await ReadRepository.GetAsync(ids);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await ReadRepository.UpdateAsync(entity);
        }
    }
}