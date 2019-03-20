using System;

namespace ShoppingCartApi.Controllers.Calculator
{
    public class ShoppingItemDto
    {
        public Guid Id { get; set; }
        
        public double Quantity { get; set; }
    }
}