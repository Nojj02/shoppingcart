using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartApi.Controllers.Calculator
{
    public class CalculatorController : Controller
    {
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