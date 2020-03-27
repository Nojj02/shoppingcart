using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;

namespace ShoppingCartHandlers.Handlers
{
    public class ItemTypeReadModelHandler : IEventHandler
    {
        public async Task Handle(IList<object> newEvents)
        {
            var itemTypeRepository = new ItemTypeReadRepository(Database.ConnectionString);
            foreach (var newEvent in newEvents)
            {
                if (newEvent is ItemTypeCreatedEvent itemTypeCreatedEvent)
                {
                    var newItemType = new ItemTypeReadModel
                    {
                        Id = itemTypeCreatedEvent.Id,
                        Code = itemTypeCreatedEvent.Code
                    };
                    await itemTypeRepository.SaveAsync(newItemType);
                }
            }
        }
    }
}
