using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Api.DataAccess
{
    public class ItemTypeRepository
    {
        private readonly List<string> _itemTypeCodes = new List<string>();

        public void Save(string code)
        {
            _itemTypeCodes.Add(code);
        }

        public string Get(string code)
        {
            return _itemTypeCodes.SingleOrDefault(x => x == code);
        }
    }
}