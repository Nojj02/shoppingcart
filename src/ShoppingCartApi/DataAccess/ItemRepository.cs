using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Controllers.Item;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository : IItemRepository
    {
        private const string ConnectionString = "Server=postgres;Port=5432;Database=postgres;User Id=postgres;Password=thepassword;";

        public async Task Save(Item item)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
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

        public async Task<Item> Get(string code)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
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

                return JsonConvert.DeserializeObject<Item>(content);
            }
        }
    }
}