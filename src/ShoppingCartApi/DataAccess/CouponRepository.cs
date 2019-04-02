using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public class CouponRepository : Repository<Coupon, ICouponEvent>, ICouponRepository
    {
        public CouponRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "coupon";
        
        protected override Coupon MapEventsToEntity(Guid id, IReadOnlyList<ICouponEvent> events) => new Coupon(id, events);

        public async Task<Coupon> GetAsync(string code)
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

                return content == null ? null : JsonConvert.DeserializeObject<Coupon>(content);
            }
        }
    }
}