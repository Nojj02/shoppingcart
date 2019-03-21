using System;

namespace ShoppingCartApi.Controllers.Item
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public double PercentOff { get; set; }
        public decimal AmountOff { get; set; }
    }
}