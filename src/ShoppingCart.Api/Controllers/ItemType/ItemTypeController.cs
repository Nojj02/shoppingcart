using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.DataAccess;

namespace ShoppingCart.Api.Controllers.ItemType
{
    [Route("itemType")]
    public class ItemTypeController : Controller
    {
        private readonly ItemTypeRepository _repository;

        public ItemTypeController(ItemTypeRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ObjectResult> Post(PostNewItemTypeDto postNewItemTypeDto)
        {
            _repository.Save(postNewItemTypeDto.Code);
            return Created(Url.Action("GetByItemTypeCode"), new ItemTypeDto
            {
                Code = postNewItemTypeDto.Code
            });
        }

        [HttpGet]
        public async Task<ObjectResult> GetByItemTypeCode(string code)
        {
            var entity = _repository.Get(code);
            if (entity == null)
            {
                return NotFound(code);
            }

            return Ok(
                new ItemTypeDto
                {
                    Code = entity
                });
        }
    }
}