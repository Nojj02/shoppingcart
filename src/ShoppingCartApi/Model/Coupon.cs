using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class Coupon
    {
        public Coupon(
            Guid id,
            string code, 
            decimal amountOff,
            Percentage percentOff)
        {
            Id = id;
            Code = code;
            AmountOff = amountOff;
            PercentOff = percentOff;
        }

        public Guid Id { get; }

        public string Code { get; }

        public decimal AmountOff { get; }

        public Percentage PercentOff { get; }
    }
}
