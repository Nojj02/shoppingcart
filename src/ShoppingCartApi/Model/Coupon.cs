using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShoppingCartApi.Model
{
    public class Coupon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="percentOff"></param>
        /// <param name="forItemTypeId">If null, means the coupon applies store-wide</param>
        public Coupon(
            Guid id,
            string code, 
            Percentage percentOff,
            Guid? forItemTypeId = null)
        {
            Id = id;
            Code = code;
            PercentOff = percentOff;
            ForItemTypeId = forItemTypeId;
        }

        public Guid Id { get; }

        public string Code { get; }

        public Percentage PercentOff { get; }

        public Guid? ForItemTypeId { get; private set; }
    }
}
