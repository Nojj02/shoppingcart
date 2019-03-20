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

        [HttpPost]
        [Route("computeCost")]
        public async Task<ObjectResult> ComputeCost([FromBody]CalculatorComputeCostRequestDto requestDto)
        {
            var shoppingItemCodes =
                requestDto.ShoppingItems
                    .Select(shoppingItemDto => shoppingItemDto.ItemCode)
                    .ToList();

            var allMatchingItems = await _itemRepository.GetAsync(shoppingItemCodes);

            var totalCost =
                requestDto.ShoppingItems
                    .Sum(shoppingItem =>
                    {
                        var item = allMatchingItems.SingleOrDefault(matchingItem => matchingItem.Code == shoppingItem.ItemCode);

                        if (item == null) return 0;

                        var percentageDiscount = item.PercentOff / 100;

                        return item.Price * Convert.ToDecimal(shoppingItem.Quantity) * Convert.ToDecimal(1 - percentageDiscount);
                    });
            
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = totalCost
            });
        }
    }
}