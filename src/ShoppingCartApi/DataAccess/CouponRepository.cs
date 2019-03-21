using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.DataAccess
{
    public class CouponRepository : ICouponRepository
    {
        private readonly string _connectionString;

        public CouponRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveAsync(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO shoppingcart.coupon (id, content, timestamp) VALUES (@id, @content::jsonb, @timestamp)",
                    new
                    {
                        id = coupon.Id,
                        content = JsonConvert.SerializeObject(coupon),
                        timestamp = DateTimeOffset.UtcNow
                    });
            }
        }

        public async Task<Coupon> GetByCouponCodeAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.coupon " +
                        "WHERE content->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<Coupon>(content);
            }
        }

        public async Task<Coupon> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content =
                    await connection.QuerySingleOrDefaultAsync<string>(
                        "SELECT content " +
                        "FROM shoppingcart.coupon " +
                        "WHERE id = @id",
                        new
                        {
                            id = id
                        });

                return content == null ? null : JsonConvert.DeserializeObject<Coupon>(content);
            }
        }
    }
}