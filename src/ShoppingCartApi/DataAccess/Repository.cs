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

        public async Task<T> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        $@"SELECT content 
                            FROM {SchemaAndTableName}
                            WHERE id = @id",
                        new
                        {
                            id = id
                        });

                return content == null ? null : JsonConvert.DeserializeObject<T>(content);
            }
        }

        public async Task<IReadOnlyList<T>> GetAsync(IEnumerable<Guid> ids)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var content =
                    await connection.QueryAsync<string>(
                        $@"SELECT content 
                            FROM {SchemaAndTableName}
                            WHERE id = ANY(@ids)",
                        new
                        {
                            ids = ids
                        });

                return content.Select(JsonConvert.DeserializeObject<T>).ToList();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(
                    $@"UPDATE {SchemaAndTableName}
                        SET content = @content::jsonb,
                            timestamp = @timestamp 
                        WHERE id = @id",
                    new
                    {
                        id = entity.Id,
                        content = JsonConvert.SerializeObject(entity),
                        timestamp = DateTimeOffset.UtcNow
                    });
            }
        }
    }
}