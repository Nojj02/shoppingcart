using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Utilities.CustomActionResults;

namespace ShoppingCartApi.Controllers.Item
{
    [Route("items")]
    public class ItemController : Controller
    {
        private readonly IItemRepository _repository;

        public ItemController(IItemRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("")]
        public async Task<ObjectResult> Post([FromBody]PostRequestDto postRequestDto)
        {
            var existingItem = await _repository.GetAsync(postRequestDto.Code);
            if (existingItem != null)
            {
                var selfUrl = Url.Action("GetByItemCode", new { code = existingItem.Code });
               
                return new SeeOtherObjectResult(selfUrl, new ItemDto
                {
                    Code = existingItem.Code,
                    Price = existingItem.Price
                });
            }
            else
            {
                var entity = new Controllers.Item.Item(
                    id: Guid.NewGuid(),
                    code: postRequestDto.Code,
                    price: postRequestDto.Price);
                await _repository.SaveAsync(entity);

                var selfUrl = Url.Action("GetByItemCode", new { code = entity.Code });
                return Created(selfUrl, new ItemDto
                {
                    Code = entity.Code,
                    Price = entity.Price
                });
            }
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<ObjectResult> GetByItemCode(string code)
        {
            var entity = await _repository.GetAsync(code);
            if (entity == null)
            {
                return NotFound(code);
            }

            return Ok(
                new ItemDto
                {
                    Code = entity.Code,
                    Price = entity.Price
                });
        }
    }
}