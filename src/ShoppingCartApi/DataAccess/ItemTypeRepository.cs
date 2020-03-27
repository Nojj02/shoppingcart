using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartApi.Model;
using ShoppingCartEvents;

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
    }
}