using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository : IItemRepository
    {
        private string _connectionString;
        
        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveAsync(Item item)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO shoppingcart.item (id, content, timestamp) VALUES (@id, @content::jsonb, @timestamp)",
                    new
                    {
                        id = item.Id,
                        content = JsonConvert.SerializeObject(item),
                        timestamp = DateTimeOffset.UtcNow
                    });
            }
        }

        public async Task<Item> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content = 
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.item " +
                        "WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<Item>(content);
            }
        }

        public async Task<IReadOnlyList<Item>> GetAsync(IEnumerable<string> codes)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content =
                    await connection.QueryAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.item " +
                        "WHERE content->>'Code' = ANY(@codes)",
                        new
                        {
                            codes = codes.ToArray()
                        });

                return content.Select(JsonConvert.DeserializeObject<Item>).ToList();
            }
        }

        public async Task UpdateAsync(Item entity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "UPDATE shoppingcart.item SET " +
                    "content = @content::jsonb " +
                    ", timestamp = @timestamp " +
                    "WHERE id = @id",
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