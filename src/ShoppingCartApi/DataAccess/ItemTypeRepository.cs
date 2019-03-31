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
    public class ItemTypeRepository : Repository<ItemType, IItemTypeEvent>, IItemTypeRepository
    {
        public ItemTypeRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "item_type";
        
        protected override ItemType MapEventsToEntity(Guid id, IReadOnlyList<IItemTypeEvent> events) => new ItemType(id, events);

        public async Task<ItemType> GetAsync(string code)
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

                return content == null ? null : JsonConvert.DeserializeObject<ItemType>(content);
            }
        }
    }
}