using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ShoppingCartApi.DataAccess;

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
            var existingItem = await _repository.Get(postRequestDto.Code);
            if (existingItem != null)
            {
                var selfUrl = Url.Action("GetByItemCode", new { code = existingItem.Code });
                HttpContext.Response.Headers.Add(Convert.ToString(HttpResponseHeader.Location), new StringValues(selfUrl));
                return new ObjectResult(new ItemDto
                {
                    Code = existingItem.Code,
                    Price = existingItem.Price
                })
                {
                    StatusCode = (int)HttpStatusCode.SeeOther
                };
            }
            else
            {
                var entity = new Controllers.Item.Item(
                    id: Guid.NewGuid(),
                    code: postRequestDto.Code,
                    price: postRequestDto.Price);
                await _repository.Save(entity);

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
            var entity = await _repository.Get(code);
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