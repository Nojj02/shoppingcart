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
    }
}