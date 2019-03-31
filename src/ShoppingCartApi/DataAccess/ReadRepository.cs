using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public abstract class ReadRepository<T> : IRepository<T>
        where T : AggregateRoot
    {
        protected const string SchemaName = "shoppingcart_views";

        protected ReadRepository(string connectionString)
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
                await connection.ExecuteAsync(
                    $@"INSERT INTO {SchemaAndTableName} 
                            (id, content, timestamp) 
                        VALUES 
                            (@id, @content::jsonb, @timestamp)",
                    new
                    {
                        id = entity.Id,
                        content = JsonConvert.SerializeObject(entity),
                        timestamp = DateTimeOffset.UtcNow
                    });
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