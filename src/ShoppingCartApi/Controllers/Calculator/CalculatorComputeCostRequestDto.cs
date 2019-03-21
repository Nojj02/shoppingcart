using System.Collections.Generic;

namespace ShoppingCartApi.Controllers.Calculator
{
    public class CalculatorComputeCostRequestDto
    {
        public CalculatorComputeCostRequestDto()
        {
            ShoppingItems = new List<ShoppingItemDto>();
        }
        
        public IEnumerable<ShoppingItemDto> ShoppingItems { get; set; }

        public string CouponCode { get; set; }
    }
}