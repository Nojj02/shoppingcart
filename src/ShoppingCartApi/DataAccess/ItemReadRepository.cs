using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class ItemReadRepository : ReadRepository<Item>, IItemRepository
    {
        private readonly string _connectionString;
        
        public ItemReadRepository(string connectionString)
            : base(connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string TableName => "item";

        public async Task<Item> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content = 
                    await connection.QuerySingleOrDefaultAsync<string>(
                        $@"SELECT event FROM {SchemaAndTableName} WHERE event->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<Item>(content);
            }
        }
    }
}