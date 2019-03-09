using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var itemRepository = new ItemRepository();
            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var result = await calculatorController.ComputeCost(new CalculatorComputeCostRequestDto());
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;
            
            Assert.Equal(0, dto.TotalCost);
        }
        
        [Fact]
        public async Task CostOfOneItem_OneShoppingItem()
        {
            var itemRepository = new ItemRepository();

            var itemController = new ItemController(itemRepository);
            BootstrapController(itemController);
            
            var postNewPotatoItemDto = new PostRequestDto
            {
                Code = "potato",
                Price = 30
            };
            
            var postNewItemResult = await itemController.Post(postNewPotatoItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewItemResult.StatusCode);
           
            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var calculatorComputeCostRequestDto =
                new CalculatorComputeCostRequestDto
                {
                    ShoppingItems = new List<ShoppingItemDto>
                    {
                        new ShoppingItemDto
                        {
                            ItemCode = "potato",
                            Quantity = 1
                        }
                    }
                };

            var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;
            
            Assert.Equal(30, dto.TotalCost);
        }
        
        [Fact]
        public async Task CostOfTwoItems_TwoShoppingItems()
        {
            var itemRepository = new ItemRepository();

            var itemController = new ItemController(itemRepository);
            BootstrapController(itemController);
            
            var postNewPotatoItemDto = new PostRequestDto
            {
                Code = "potato",
                Price = 30
            };
            
            var postNewPotatoItemResult = await itemController.Post(postNewPotatoItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewPotatoItemResult.StatusCode);
           
            var postNewLettuceItemDto = new PostRequestDto
            {
                Code = "lettuce",
                Price = 50
            };

            var postNewLettuceItemResult = await itemController.Post(postNewLettuceItemDto);
            Assert.Equal((int)HttpStatusCode.Created, postNewLettuceItemResult.StatusCode);
            
            var calculatorController = new CalculatorController(itemRepository);
            BootstrapController(calculatorController);

            var calculatorComputeCostRequestDto =
                new CalculatorComputeCostRequestDto
                {
                    ShoppingItems = new List<ShoppingItemDto>
                    {
                        new ShoppingItemDto
                        {
                            ItemCode = "potato",
                            Quantity = 1
                        },
                        new ShoppingItemDto
                        {
                            ItemCode = "lettuce",
                            Quantity = 1
                        }
                    }
                };

            var result = await calculatorController.ComputeCost(calculatorComputeCostRequestDto);
            
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var dto = (CalculatorComputeCostDto)result.Value;
            
            Assert.Equal(80, dto.TotalCost);
        }

        private void BootstrapController(Controller controller)
        {
            controller.Url = new AlwaysEmptyUrlHelper();
        }
    }
}
