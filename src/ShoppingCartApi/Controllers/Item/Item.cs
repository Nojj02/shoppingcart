using System;
using Newtonsoft.Json;

namespace ShoppingCartApi.Controllers.Item
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
        }

        [JsonConstructor]
        private Item(
            Guid id,
            string code,
            decimal price,
            double percentOff)
        {
            Id = id;
            Code = code;
            Price = price;
            PercentOff = percentOff;
        }

        public Guid Id { get; }
        
        public string Code { get; }

        public decimal Price { get; }
        
        public double PercentOff { get; private set; }

        public void SetDiscount(double percentOff)
        {
            PercentOff = percentOff;
        }
    }
}