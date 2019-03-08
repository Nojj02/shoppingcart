using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using ShoppingCartApi.Controllers;
using ShoppingCartApi.Controllers.Calculator;
using ShoppingCartApi.Controllers.Item;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Tests.Helpers;
using Xunit;

namespace ShoppingCartApi.Tests
{
    public class CalculatorControllerTests
    {
        [Fact]
        public async Task CostIsZero_NoShoppingItems()
        {
            var calculatorController = new CalculatorController();
            BootstrapController(calculatorController);

            var result = await calculatorController.ComputeCost();
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (ComputeCostDto)result.Value;
            
            Assert.Equal(0, dto.TotalCost);
        }

        private void BootstrapController(CalculatorController controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
