using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi.Controllers.Item
{
    [Route("items")]
    public class ItemController : Controller
    {
        private readonly ItemRepository _repository;

        public ItemController(ItemRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ObjectResult> Post(PostNewItemDto postNewItemDto)
        {
            _repository.Save(postNewItemDto.Code);
            return Created(Url.Action("GetByItemCode"), new ItemDto
            {
                Code = postNewItemDto.Code
            });
        }

        [HttpGet("{code}")]
        public async Task<ObjectResult> GetByItemCode(string code)
        {
            var entity = _repository.Get(code);
            if (entity == null)
            {
                return NotFound(code);
            }

            return Ok(
                new ItemDto
                {
                    Code = entity
                });
        }
    }
}