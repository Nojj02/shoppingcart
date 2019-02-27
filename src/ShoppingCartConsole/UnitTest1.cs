using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShoppingCartConsole
{
    public class ShopTests
    {
        [Fact]
        public void CanEnterSeveralShoppingItems()
        {
            var shop = StartNewShop();

            var item = shop.GetItem(itemCode: "banana");
            var item2 = shop.GetItem(itemCode: "potato");

            var shoppingItems = new List<ShoppingItem>
            {
                new ShoppingItem(item, quantity: 3),
                new ShoppingItem(item2, quantity: 1)
            };

            var totalCost = shop.ComputeCost(shoppingItems);

            Assert.Equal(160, totalCost);
        }

        private Shop StartNewShop()
        {
            var item = new Item(code: "banana" ,price: 30);
            var item2 = new Item(code: "potato", price: 70);
            var items = new List<Item>
            {
                item,
                item2
            };

            return new Shop(items);
        }
    }

    public class ShoppingItem
    {
        public ShoppingItem(Item item, double quantity)
        {
            Quantity = quantity;
            Cost = item.Price;
        }

        public decimal Cost { get; }
        
        public double Quantity { get; }
    }

    public class Shop
    {
        public Shop(IReadOnlyList<Item> items)
        {
            Items = new List<Item>(items);
        }

        public IReadOnlyList<Item> Items { get; }

        public decimal ComputeCost(List<ShoppingItem> shoppingItems)
        {
            return shoppingItems.Sum(x => x.Cost * Convert.ToDecimal(x.Quantity));
        }

        public Item GetItem(string itemCode)
        {
            return Items.SingleOrDefault(x => x.Code == itemCode);
        }
    }

    public class Item
    {
        public Item(string code, decimal price)
        {
            Code = code;
            Price = price;
        }

        public string Code { get; }
        
        public decimal Price { get; }
    }
}