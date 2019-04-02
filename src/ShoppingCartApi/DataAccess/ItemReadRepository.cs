using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public interface IItemReadRepository
    {
        Task<ItemReadModel> GetAsync(string code);
    }

    public class ItemReadRepository : ReadRepository<Item>, IItemReadRepository
    {
        private readonly string _connectionString;
        
        public ItemReadRepository(string connectionString)
            : base(connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string TableName => "item";

        public async Task<ItemReadModel> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content = 
                    await connection.QuerySingleOrDefaultAsync<string>(
                        $@"SELECT content FROM {SchemaAndTableName} WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<ItemReadModel>(content);
            }
        }
    }
}