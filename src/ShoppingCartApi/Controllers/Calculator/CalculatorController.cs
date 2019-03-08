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
        public async Task<ObjectResult> ComputeCost()
        {
            return Ok(new CalculatorComputeCostDto
            {
                TotalCost = 0
            });
        }
    }
}