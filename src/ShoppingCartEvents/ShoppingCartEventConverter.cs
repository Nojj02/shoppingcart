using System;
using System.Collections.Generic;

namespace ShoppingCartEvents
{
    public class ShoppingCartEventConverter : EventConverter
    {
        private static Lazy<ShoppingCartEventConverter> _instance => new Lazy<ShoppingCartEventConverter>(() => new ShoppingCartEventConverter());

        public static ShoppingCartEventConverter Instance => _instance.Value;

        public ShoppingCartEventConverter() 
            : base(new Dictionary<string, Type>
            {
                { "coupon-created", typeof(CouponCreatedEvent) },
                { "itemType-created", typeof(ItemTypeCreatedEvent) },
                { "item-created", typeof(ItemCreatedEvent) },
                { "item-amountDiscountSet", typeof(ItemAmountDiscountSetEvent) },
                { "item-percentageDiscountSet", typeof(ItemPercentageDiscountSetEvent) }
            })
        {

        }
    }
}