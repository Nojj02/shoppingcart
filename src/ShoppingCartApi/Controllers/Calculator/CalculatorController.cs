using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi.Controllers.Calculator
{
    [Route("calculator")]
    public class CalculatorController : Controller
    {
        private readonly IItemRepository _itemRepository;

        public CalculatorController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET
        [HttpPost("computeCost")]
        public async Task<ObjectResult> ComputeCost([FromBody]CalculatorComputeCostRequestDto requestDto)
        {

            var totalCost =
                requestDto.ShoppingItems
                    .Sum(shoppingItem =>
                    {
                        var item = _itemRepository.Get(shoppingItem.ItemCode).GetAwaiter().GetResult();

                        return item != null ? item.Price * Convert.ToDecimal(shoppingItem.Quantity) : 0;
                    });
            
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = totalCost
            });
        }
    }
}