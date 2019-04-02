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
    public abstract class Repository<T, TEvent> : IRepository<T>
        where T : AggregateRoot<TEvent>
        where TEvent : IEvent
    {
        protected const string SchemaName = "shoppingcart";
        protected const string ViewsSchemaName = "shoppingcart_views";

        protected Repository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected abstract string TableName { get; }

        protected string ConnectionString { get; }

        protected string SchemaAndTableName => $"{SchemaName}.{TableName}";
        protected string ViewsSchemaAndTableName => $"{ViewsSchemaName}.{TableName}";

        public async Task SaveAsync(T entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(
                            $@"INSERT INTO {ViewsSchemaAndTableName} 
                            (id, content, timestamp) 
                        VALUES 
                            (@id, @content::jsonb, @timestamp)",
                            new
                            {
                                id = entity.Id,
                                content = JsonConvert.SerializeObject(entity),
                                timestamp = DateTimeOffset.UtcNow
                            });
                        
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
                        await connection.ExecuteAsync(
                            $@"UPDATE {ViewsSchemaAndTableName}
                                SET content = @content::jsonb,
                                    timestamp = @timestamp 
                                WHERE id = @id",
                            new
                            {
                                id = entity.Id,
                                content = JsonConvert.SerializeObject(entity),
                                timestamp = DateTimeOffset.UtcNow
                            });
                        
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

        protected abstract T MapEventsToEntity(Guid id, IReadOnlyList<TEvent> events);
    }
}