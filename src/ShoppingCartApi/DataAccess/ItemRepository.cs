using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository : Repository<Item, IItemEvent>, IItemRepository
    {
        public ItemRepository(string connectionString)
            : base(connectionString)
        {
        }

        protected override string TableName => "item";
        
        protected override Item MapEventsToEntity(Guid id, IReadOnlyList<IItemEvent> events) => new Item(id, events);

        public Task<Item> GetAsync(string code)
        {
            throw new NotSupportedException();
        }
    }
}