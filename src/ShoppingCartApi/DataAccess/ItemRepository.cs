using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository : Repository<Item, IItemEvent>, IItemRepository
    {
        private readonly string _connectionString;
        
        public ItemRepository(string connectionString)
            : base(connectionString)
        {
            _connectionString = connectionString;
        }

        protected override string TableName => "item";
        
        protected override Item MapEventsToEntity(Guid id, IReadOnlyList<IItemEvent> events) => new Item(id, events);

        public async Task<Item> GetAsync(string code)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var content = 
                    await connection.QuerySingleOrDefaultAsync<string>(
                        $@"SELECT event FROM shoppingcart.item WHERE event->>'Code' = @code",
                        new
                        {
                            code = code
                        });

                return content == null ? null : JsonConvert.DeserializeObject<Item>(content);
            }
        }
    }
}