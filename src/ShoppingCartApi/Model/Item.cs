using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using ShoppingCartApi.Model.Events;

namespace ShoppingCartApi.Model
{
    public class Item : AggregateRoot
    {
        private readonly List<IItemEvent> _events = new List<IItemEvent>();
        private readonly List<IItemEvent> _newEvents = new List<IItemEvent>();

        public Item(
            Guid id,
            string code,
            decimal price,
            Guid itemTypeId)
            : base(id)
        {
            var itemCreatedEvent = new ItemCreatedEvent
            {
                Id = id,
                Code = code,
                Price = price,
                ItemTypeId = itemTypeId
            };
            Apply(itemCreatedEvent, isNew: true);
        }

        private Item(Guid id, IReadOnlyList<IItemEvent> itemEvents)
            : base(id)
        {
            foreach (var itemEvent in itemEvents)
            {
                switch (itemEvent)
                {
                    case ItemCreatedEvent itemCreatedEvent:
                        Apply(itemCreatedEvent);
                        break;
                    case ItemPercentageDiscountSetEvent itemPercentageDiscountSetEvent:
                        Apply(itemPercentageDiscountSetEvent);
                        break;
                    case ItemAmountDiscountSetEvent itemAmountDiscountSetEvent:
                        Apply(itemAmountDiscountSetEvent);
                        break;
                }
            }
        }

        public IReadOnlyList<IItemEvent> Events => _events;

        public IReadOnlyList<IItemEvent> NewEvents => _newEvents;

        public string Code { get; private set; }

        public Guid ItemTypeId { get; private set; }

        public decimal Price { get; private set; }

        public Percent PercentOff { get; private set; }

        public decimal AmountOff { get; private set; }

        public void SetPercentageDiscount(Percent percentOff)
        {
            var itemPercentageDiscountSetEvent = new ItemPercentageDiscountSetEvent
            {
                PercentOff = percentOff
            };

            Apply(itemPercentageDiscountSetEvent, isNew: true);
        }

        public void SetAmountDiscount(decimal amountOff)
        {
            var itemAmountDiscountSetEvent = new ItemAmountDiscountSetEvent
            {
                AmountOff = amountOff
            };

            Apply(itemAmountDiscountSetEvent, isNew: true);
        }

        private void Apply(ItemCreatedEvent itemCreatedEvent, bool isNew = false)
        {
            AddEvent(itemCreatedEvent, isNew);

            Code = itemCreatedEvent.Code;
            Price = itemCreatedEvent.Price;
            ItemTypeId = itemCreatedEvent.ItemTypeId;
            PercentOff = Percent.Zero;
        }

        private void Apply(ItemPercentageDiscountSetEvent itemPercentageDiscountSetEvent, bool isNew = false)
        {
            AddEvent(itemPercentageDiscountSetEvent, isNew);

            PercentOff = itemPercentageDiscountSetEvent.PercentOff;
        }

        private void Apply(ItemAmountDiscountSetEvent itemAmountDiscountSetEvent, bool isNew = false)
        {
            AddEvent(itemAmountDiscountSetEvent, isNew);

            AmountOff = itemAmountDiscountSetEvent.AmountOff;
        }

        private void AddEvent(IItemEvent anEvent, bool isNew)
        {
            _events.Add(anEvent);
            if (isNew)
            {
                _newEvents.Add(anEvent);
            }
        }

        public static Item Reconstitute(Guid id, IReadOnlyList<IItemEvent> itemEvents)
        {
            return EnumerableExtensions.Any(itemEvents) ? new Item(id, itemEvents) : null;
        }
    }
}