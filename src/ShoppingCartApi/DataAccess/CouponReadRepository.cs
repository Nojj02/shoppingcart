using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class CouponReadRepository : ReadRepository<CouponReadModel>, ICouponReadRepository
    {
        public CouponReadRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "coupon";
        
        public async Task<CouponReadModel> GetAsync(string code)
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

                return content == null ? null : JsonConvert.DeserializeObject<CouponReadModel>(content);
            }
        }
    }
}