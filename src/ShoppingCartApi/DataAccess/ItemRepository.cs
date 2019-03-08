using System.Collections.Generic;
using System.Linq;
using ShoppingCartApi.Controllers.Item;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository
    {
        private readonly List<Item> _items = new List<Item>();

        public void Save(Item item)
        {
            _items.Add(item);
        }

        public Item Get(string code)
        {
            return _items.SingleOrDefault(x => x.Code == code);
        }
    }
}