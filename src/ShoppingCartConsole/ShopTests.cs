using System.Collections.Generic;
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
        public void ItemsCanOnSpecialPercent()
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

        [Fact]
        public void ItemsCanOnSpecial()
        {
            var shop = StartNewShop();

            var item = shop.GetItem(itemCode: "banana");
            item.SetAmountDiscount(amount: 15);
            
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
}