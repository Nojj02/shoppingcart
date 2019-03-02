using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartConsole
{
    public class Shop
    {
        public Shop(IReadOnlyList<Item> items)
        {
            Items = new List<Item>(items);
        }

        public IReadOnlyList<Item> Items { get; }

        public decimal ComputeCost(List<ShoppingItem> shoppingItems)
        {
            return shoppingItems.Sum(x =>
            {
                var matchingItem = Items.SingleOrDefault(item => item.Code == x.ItemCode);
                if (matchingItem == null) return 0;
                
                return matchingItem.DiscountedPrice * Convert.ToDecimal(x.Quantity);
            });
        }

        public Item GetItem(string itemCode)
        {
            return Items.SingleOrDefault(x => x.Code == itemCode);
        }
    }
}