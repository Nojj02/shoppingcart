using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TReadModel">The Read Model of the same Aggregate Root</typeparam>
    public abstract class ReadRepository<TReadModel>
        where TReadModel : class, IEntity
    {
        protected const string SchemaName = "shoppingcart_views";

        protected ReadRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        protected abstract string TableName { get; }

        protected string ConnectionString { get; }

        protected string SchemaAndTableName => $"{SchemaName}.{TableName}";

        public async Task<TReadModel> GetAsync(Guid id)
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

                return content == null ? null : JsonConvert.DeserializeObject<TReadModel>(content);
            }
        }

        public async Task<IReadOnlyList<TReadModel>> GetAsync(IEnumerable<Guid> ids)
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

                return content.Select(JsonConvert.DeserializeObject<TReadModel>).ToList();
            }
        }

        public async Task SaveAsync(TReadModel entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
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

        public async Task UpdateAsync(TReadModel entity)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
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
    }
}