using System;

namespace ShoppingCartConsole
{
    public class Item
    {
        public Item(string code, decimal price)
        {
            Code = code;
            Price = price;
            DiscountedPrice = price;
        }

        public string Code { get; }
        
        public decimal Price { get; }

        public void SetDiscount(double percentDiscount)
        {
            var discountedPrice = Price * Convert.ToDecimal(percentDiscount / 100);
            DiscountedPrice = Price - discountedPrice;
        }

        public double PercentDiscount { get; set; }

        public void SetAmountDiscount(decimal amount)
        {
            DiscountedPrice = Price - amount;
        }
        
        public decimal DiscountedPrice { get; set; }
    }
}