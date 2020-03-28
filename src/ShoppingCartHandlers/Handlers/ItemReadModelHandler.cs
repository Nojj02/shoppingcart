using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartEvents;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;
using ShoppingCartSharedKernel;

namespace ShoppingCartHandlers.Handlers
{
    public class ItemReadModelHandler : IEventHandler
    {
        private readonly IItemReadRepository _itemReadRepository;

        public ItemReadModelHandler(IItemReadRepository itemReadRepository)
        {
            _itemReadRepository = itemReadRepository;
        }

        public async Task Handle(IList<object> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                switch (newEvent)
                {
                    case ItemCreatedEvent itemCreatedEvent:
                    {
                        var item = new ItemReadModel
                        {
                            Id = itemCreatedEvent.Id,
                            Code = itemCreatedEvent.Code,
                            ItemTypeId = itemCreatedEvent.ItemTypeId,
                            Price = itemCreatedEvent.Price,
                            PercentOff = Percent.Zero
                        };
                        await _itemReadRepository.SaveAsync(item);
                        break;
                    }
                    case ItemAmountDiscountSetEvent itemAmountDiscountSetEvent:
                    {
                        var item = await _itemReadRepository.GetAsync(itemAmountDiscountSetEvent.Id);
                        item.AmountOff = itemAmountDiscountSetEvent.AmountOff;
                        await _itemReadRepository.UpdateAsync(item);
                        break;
                    }
                    case ItemPercentageDiscountSetEvent itemPercentageDiscountSetEvent:
                    {
                        var item = await _itemReadRepository.GetAsync(itemPercentageDiscountSetEvent.Id);
                        item.PercentOff = itemPercentageDiscountSetEvent.PercentOff;
                        await _itemReadRepository.UpdateAsync(item);
                        break;
                    }
                }
            }
        }
    }
}