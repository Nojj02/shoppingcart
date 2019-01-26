using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApi.DataAccess
{
    public class ItemRepository
    {
        private readonly List<string> _itemCodes = new List<string>();

        public void Save(string code)
        {
            _itemCodes.Add(code);
        }

        public string Get(string code)
        {
            return _itemCodes.SingleOrDefault(x => x == code);
        }
    }
}