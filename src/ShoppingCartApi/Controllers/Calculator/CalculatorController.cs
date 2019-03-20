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
            var shoppingItemIds =
                requestDto.ShoppingItems
                    .Select(shoppingItemDto => shoppingItemDto.Id)
                    .ToList();

            var allMatchingItems = await _itemRepository.GetAsync(shoppingItemIds);

            var totalCost =
                requestDto.ShoppingItems
                    .Sum(shoppingItem =>
                    {
                        var item = allMatchingItems.SingleOrDefault(matchingItem => matchingItem.Id == shoppingItem.Id);

                        if (item == null) return 0;

                        var grossAmount = item.Price * Convert.ToDecimal(shoppingItem.Quantity);

                        var discountedAmount = item.PercentOff.Of(grossAmount);

                        return grossAmount - discountedAmount;
                    });
            
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = totalCost
            });
        }
    }
}