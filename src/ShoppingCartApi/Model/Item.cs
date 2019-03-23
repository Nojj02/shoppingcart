using System;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class Item
    {
        public Item(
            Guid id,
            string code,
            decimal price,
            Guid itemTypeId)
        {
            Id = id;
            Code = code;
            Price = price;
            ItemTypeId = itemTypeId;
            PercentOff = Percent.Zero;
        }

        [JsonConstructor]
        private Item(
            Guid id,
            string code,
            Guid itemTypeId,
            decimal price,
            Percent percentOff,
            decimal amountOff)
        {
            Id = id;
            Code = code;
            ItemTypeId = itemTypeId;
            Price = price;
            PercentOff = percentOff;
            AmountOff = amountOff;
        }

        public Guid Id { get; }
        
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