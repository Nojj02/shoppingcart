using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public abstract class Repository<T, TEvent> : IRepository<T, TEvent>
        where T : AggregateRoot<TEvent>
        where TEvent : IEvent
    {
        protected const string SchemaName = "shoppingcart";

        protected Repository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected abstract string TableName { get; }

        protected string ConnectionString { get; }

        protected string SchemaAndTableName => $"{SchemaName}.{TableName}";

        public async Task SaveAsync(T entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var newEvent in entity.Events)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO {SchemaAndTableName} 
                            (id, version, event_type, event, timestamp) 
                        VALUES 
                            (@id, @version, @event_type, @event_content::jsonb, @timestamp)",
                                new
                                {
                                    id = entity.Id,
                                    version = newEvent.Version,
                                    event_type = newEvent.GetType().FullName,
                                    event_content = JsonConvert.SerializeObject(newEvent),
                                    timestamp = DateTimeOffset.UtcNow
                                });
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task SaveAsync(T entity, DateTimeOffset timestamp)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var newEvent in entity.Events)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO {SchemaAndTableName} 
                            (id, version, event_type, event, timestamp) 
                        VALUES 
                            (@id, @version, @event_type, @event_content::jsonb, @timestamp)",
                                new
                                {
                                    id = entity.Id,
                                    version = newEvent.Version,
                                    event_type = newEvent.GetType().FullName,
                                    event_content = JsonConvert.SerializeObject(newEvent),
                                    timestamp = timestamp
                                });
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<IReadOnlyList<TEvent>> GetEventsAsync(int startIndex, int endIndex)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var eventStrings =
                    await connection.QueryAsync<dynamic>(
                        $@"SELECT event as event_content, event_type
                            FROM {SchemaAndTableName}
                            LIMIT @limit OFFSET @offset",
                        new
                        {
                            offset = startIndex,
                            limit = endIndex - startIndex + 1
                        });
                var events = eventStrings
                    .Select(x => (TEvent)JsonConvert.DeserializeObject(x.event_content, Type.GetType(x.event_type)))
                    .ToList();

                return events;
            }
        }

        public async Task<T> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var eventStrings =
                    await connection.QueryAsync<dynamic>(
                        $@"SELECT event as event_content, event_type
                            FROM {SchemaAndTableName}
                            WHERE id = @id",
                        new
                        {
                            id = id
                        });
                var events = eventStrings
                    .Select(x => (TEvent)JsonConvert.DeserializeObject(x.event_content, Type.GetType(x.event_type)))
                    .ToList();

                return MapEventsToEntity(id, events);
            }
        }

        public async Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var eventStrings =
                    await connection.QueryAsync<dynamic>(
                        $@"SELECT event as event_content, event_type
                            FROM {SchemaAndTableName}
                            WHERE id = ANY(@ids)",
                        new
                        {
                            ids = ids
                        });
                
                var events = eventStrings
                    .Select(x => (TEvent)JsonConvert.DeserializeObject(x.event_content, Type.GetType(x.event_type)))
                    .ToList()
                    .GroupBy(x => x.Id)
                    .Select(x => MapEventsToEntity(x.Key, x.ToList()))
                    .ToList();

                return events;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var newEvent in entity.NewEvents)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO {SchemaAndTableName} 
                                        (id, version, event_type, event, timestamp) 
                                    VALUES 
                                        (@id, @version, @event_type, @event_content::jsonb, @timestamp)",
                                new
                                {
                                    id = entity.Id,
                                    version = newEvent.Version,
                                    event_type = newEvent.GetType().FullName,
                                    event_content = JsonConvert.SerializeObject(newEvent),
                                    timestamp = DateTimeOffset.UtcNow
                                });
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateAsync(T entity, DateTimeOffset timestamp)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var newEvent in entity.NewEvents)
                        {
                            await connection.ExecuteAsync(
                                $@"INSERT INTO {SchemaAndTableName} 
                                        (id, version, event_type, event, timestamp) 
                                    VALUES 
                                        (@id, @version, @event_type, @event_content::jsonb, @timestamp)",
                                new
                                {
                                    id = entity.Id,
                                    version = newEvent.Version,
                                    event_type = newEvent.GetType().FullName,
                                    event_content = JsonConvert.SerializeObject(newEvent),
                                    timestamp = timestamp
                                });
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        protected abstract T MapEventsToEntity(Guid id, IReadOnlyList<TEvent> events);
    }
}