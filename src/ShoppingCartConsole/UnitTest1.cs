using System;
using System.Collections.Generic;
using Xunit;

namespace ShoppingCartConsole
{
    public class ShopTests
    {
        [Fact]
        public void ShopHasAtLeast10Items()
        {
            var items = new List<Item>
            {
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item(),
                new Item()
            };
            
            var shop = new Shop(items);
            
            Assert.Equal(10, shop.Items.Count);
        }
    }

    public class Shop
    {
        public Shop(IReadOnlyList<Item> items)
        {
            Items = new List<Item>(items);
        }

        public IReadOnlyList<Item> Items { get; }
    }

    public class Item
    {
    }
}