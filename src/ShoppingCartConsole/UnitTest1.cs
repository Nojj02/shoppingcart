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

        [Fact]
        public void ItemsCanOnSpecial()
        {
            var shop = StartNewShop();

            var item = shop.GetItem(itemCode: "banana");
            item.SetDiscount(percentDiscount: 50);
            
            var item2 = shop.GetItem(itemCode: "potato");
            var item3 = shop.GetItem(itemCode: "tomato");

            var shoppingItems = new List<ShoppingItem>
            {
                new ShoppingItem(item, quantity: 3),
                new ShoppingItem(item2, quantity: 1),
                new ShoppingItem(item3, quantity: 2)
            };

            var totalCost = shop.ComputeCost(shoppingItems);

            Assert.Equal(125, totalCost);
        }

        private Shop StartNewShop()
        {
            var item = new Item(code: "banana" ,price: 30);
            var item2 = new Item(code: "potato", price: 70);
            var item3 = new Item(code: "tomato", price: 5);
            var items = new List<Item>
            {
                item,
                item2,
                item3
            };

            return new Shop(items);
        }
    }

    public class ShoppingItem
    {
        public ShoppingItem(Item item, double quantity)
        {
            ItemCode = item.Code;
            Quantity = quantity;
            Cost = item.Price;
        }

        public string ItemCode { get; set; }

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
            return shoppingItems.Sum(x =>
            {
                var matchingItem = Items.SingleOrDefault(item => item.Code == x.ItemCode);
                if (matchingItem == null) return 0;
                
                var discountedCost = matchingItem.Price * Convert.ToDecimal(matchingItem.PercentDiscount / 100);
                var cost = matchingItem.Price - discountedCost;
                
                return cost * Convert.ToDecimal(x.Quantity);
            });
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

        public void SetDiscount(int percentDiscount)
        {
            PercentDiscount = percentDiscount;
        }

        public double PercentDiscount { get; private set; }
    }
}