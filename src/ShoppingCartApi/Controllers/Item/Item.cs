using System;

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

        public Guid Id { get; }
        
        public string Code { get; }

        public decimal Price { get; }


        public double PercentageOff { get; private set; }

        public void SetDiscount(double percentageOff)
        {
            PercentageOff = percentageOff;
        }
    }
}