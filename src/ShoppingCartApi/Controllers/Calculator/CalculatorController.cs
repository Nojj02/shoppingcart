using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.Controllers.Calculator
{
    [Route("calculator")]
    public class CalculatorController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICouponRepository _couponRepository;

        public CalculatorController(IItemRepository itemRepository, ICouponRepository couponRepository)
        {
            _itemRepository = itemRepository;
            _couponRepository = couponRepository;
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
            
            var coupon = await _couponRepository.GetByCouponCodeAsync(requestDto.CouponCode);

            var percentDiscountFromCoupon = coupon != null ? coupon.PercentOff : Percentage.Zero;

            var totalCost =
                requestDto.ShoppingItems
                    .Sum(shoppingItem =>
                    {
                        var item = allMatchingItems.SingleOrDefault(matchingItem => matchingItem.Id == shoppingItem.Id);

                        if (item == null) return 0;

                        var grossAmount = item.Price * Convert.ToDecimal(shoppingItem.Quantity);

                        var discountedAmountByFixedAmount = item.AmountOff * Convert.ToDecimal(shoppingItem.Quantity);
                        var discountedAmountByPercentage = item.PercentOff.Of(grossAmount);
                        var discountedAmountFromCoupon = percentDiscountFromCoupon.Of(grossAmount);

                        var highestDiscount = new[]
                        {
                            discountedAmountByPercentage,
                            discountedAmountByFixedAmount,
                            discountedAmountFromCoupon
                        }.Max();

                        return grossAmount - highestDiscount;
                    });
            
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = totalCost
            });
        }
    }
}