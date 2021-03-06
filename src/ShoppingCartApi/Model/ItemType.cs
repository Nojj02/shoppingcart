﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingCartEvents;

namespace ShoppingCartApi.Model
{
    public class ItemType : AggregateRoot<IItemTypeEvent>, IItemType
    {
        public ItemType(
            Guid id,
            string code)
            : base(id)
        {
            Apply(
                new ItemTypeCreatedEvent
                {
                    Id = id,
                    Version = CurrentVersion + 1,
                    Code = code
                },
                isNew: true);
        }

        public ItemType(
            Guid id,
            IReadOnlyList<IItemTypeEvent> itemTypeEvents)
            : base(id, itemTypeEvents)
        {
        }

        protected override void ApplyEventsOnConstruction(IItemTypeEvent anEvent)
        {
            switch (anEvent)
            {
                case ItemTypeCreatedEvent itemTypeCreatedEvent:
                    Apply(itemTypeCreatedEvent);
                    break;
                default:
                    throw new UnsupportedEventException(anEvent.GetType());
            }
        }

        public string Code { get; private set; }

        private void Apply(ItemTypeCreatedEvent anEvent, bool isNew = false)
        {
            Code = anEvent.Code;

            base.AddEvent(anEvent, isNew);
        }
    }
}
