using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi.Controllers.Calculator
{
    public class CalculatorController : Controller
    {
        private readonly ItemRepository _itemRepository;

        public CalculatorController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET
        public async Task<ObjectResult> ComputeCost(CalculatorComputeCostRequestDto requestDto)
        {

            var totalCost =
                requestDto.ShoppingItems
                    .Sum(shoppingItem =>
                    {
                        var item = _itemRepository.Get(shoppingItem.ItemCode);

                        return item != null ? item.Price : 0;
                    });
            
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = totalCost
            });
        }
    }

    public class CalculatorComputeCostRequestDto
    {
        public CalculatorComputeCostRequestDto()
        {
            ShoppingItems = new List<ShoppingItemDto>();
        }
        
        public List<ShoppingItemDto> ShoppingItems { get; set; }
    }

    public class ShoppingItemDto
    {
        public string ItemCode { get; set; }
        
        public double Quantity { get; set; }
    }
}