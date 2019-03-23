using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class Item : AggregateRoot
    {
        private readonly List<IItemEvent> _events = new List<IItemEvent>();
        
        public Item(
            Guid id,
            string code,
            decimal price,
            Guid itemTypeId)
            : base(id)
        {
            Code = code;
            Price = price;
            ItemTypeId = itemTypeId;
            PercentOff = Percent.Zero;
            
            _events.Add(
                new ItemCreatedEvent
                {
                    Id = id,
                    Code = code,
                    Price = price,
                    ItemTypeId = itemTypeId
                });
        }

        [JsonConstructor]
        private Item(
            Guid id,
            string code,
            Guid itemTypeId,
            decimal price,
            Percent percentOff,
            decimal amountOff)
            : base(id)
        {
            Code = code;
            ItemTypeId = itemTypeId;
            Price = price;
            PercentOff = percentOff;
            AmountOff = amountOff;
        }

        public IReadOnlyList<IItemEvent> Events => _events;

        public string Code { get; }

        public Guid ItemTypeId { get; }

        public decimal Price { get; }

        public Percent PercentOff { get; private set; }

        public decimal AmountOff { get; private set; }

        public void SetPercentageDiscount(Percent percentOff)
        {
            PercentOff = percentOff;
        }

        public void SetAmountDiscount(decimal amountOff)
        {
            AmountOff = amountOff;
        }
    }
}