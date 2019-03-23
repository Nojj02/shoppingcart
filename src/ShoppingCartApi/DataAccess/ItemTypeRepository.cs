using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly string _connectionString;

        public ItemTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveAsync(ItemType itemType)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO shoppingcart.item_type (id, content, timestamp) VALUES (@id, @content::jsonb, @timestamp)",
                    new
                    {
                        id = itemType.Id,
                        content = JsonConvert.SerializeObject(itemType),
                        timestamp = DateTimeOffset.UtcNow
                    });
            }
        }

        public async Task<ItemType> GetByCodeAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.item_type " +
                        "WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<ItemType>(content);
            }
        }

        public async Task<ItemType> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.item_type " +
                        "WHERE id = @id",
                        new
                        {
                            id = id
                        });

                return content == null ? null : JsonConvert.DeserializeObject<ItemType>(content);
            }
        }
    }
}