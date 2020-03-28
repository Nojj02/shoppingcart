using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShoppingCartEvents;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;

namespace ShoppingCartHandlers.Handlers
{
    public class ItemTypeReadModelHandler : IEventHandler
    {
        private readonly IItemTypeReadRepository _itemTypeReadRepository;

        public ItemTypeReadModelHandler(IItemTypeReadRepository itemTypeReadRepository)
        {
            _itemTypeReadRepository = itemTypeReadRepository;
        }

        public async Task Handle(IList<object> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                if (newEvent is ItemTypeCreatedEvent itemTypeCreatedEvent)
                {
                    var newItemType = new ItemTypeReadModel
                    {
                        Id = itemTypeCreatedEvent.Id,
                        Code = itemTypeCreatedEvent.Code
                    };
                    await _itemTypeReadRepository.SaveAsync(newItemType);
                }
            }
        }
    }
}
