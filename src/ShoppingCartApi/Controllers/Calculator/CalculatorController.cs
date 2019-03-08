using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi.Controllers.Calculator
{
    public class CalculatorController : Controller
    {
        public CalculatorController(ItemRepository itemRepository)
        {
        }

        // GET
        public async Task<ObjectResult> ComputeCost()
        {
            return Ok(new ComputeCostDto
            {
                TotalCost = 0
            });
        }
    }

    public class ComputeCostDto
    {
        public decimal TotalCost { get; set; }
    }
}