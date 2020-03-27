using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartReader.Model;

namespace ShoppingCartReader.DataAccess
{
    public class ItemTypeReadRepository : ReadRepository<ItemTypeReadModel>, IItemTypeReadRepository
    {
        public ItemTypeReadRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "item_type";
        
        public async Task<ItemTypeReadModel> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        $"FROM {SchemaAndTableName} " +
                        "WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<ItemTypeReadModel>(content);
            }
        }
    }
}