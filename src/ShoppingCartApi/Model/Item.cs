using System;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class Item
    {
        public Item(
            Guid id,
            string code,
            decimal price)
        {
            Id = id;
            Code = code;
            Price = price;
            PercentOff = Percentage.Zero;
        }

        [JsonConstructor]
        private Item(
            Guid id,
            string code,
            decimal price,
            Percentage percentOff,
            decimal amountOff)
        {
            Id = id;
            Code = code;
            Price = price;
            PercentOff = percentOff;
            AmountOff = amountOff;
        }

        public Guid Id { get; }
        
        public string Code { get; }

        public decimal Price { get; }
        
        public Percentage PercentOff { get; private set; }

        public decimal AmountOff { get; private set; }

        public void SetPercentageDiscount(Percentage percentOff)
        {
            PercentOff = percentOff;
        }

        public void SetAmountDiscount(decimal amountOff)
        {
            AmountOff = amountOff;
        }
    }
}