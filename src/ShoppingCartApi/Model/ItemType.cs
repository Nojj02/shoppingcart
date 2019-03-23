using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class ItemType : AggregateRoot
    {
        public ItemType(
            Guid id,
            string code)
            : base(id)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
