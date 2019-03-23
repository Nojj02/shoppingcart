using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class ItemType
    {
        public ItemType(
            Guid id,
            string code)
        {
            Id = id;
            Code = code;
        }

        public Guid Id { get; }

        public string Code { get; }
    }
}
