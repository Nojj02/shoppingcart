using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.Controllers.ItemType
{
    [Route("itemTypes")]
    public class ItemTypeController : Controller
    {
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IItemTypeReadRepository _readRepository;

        public ItemTypeController(IItemTypeRepository itemTypeRepository, IItemTypeReadRepository readRepository)
        {
            _itemTypeRepository = itemTypeRepository;
            _readRepository = readRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<ObjectResult> Post([FromBody] PostRequestDto postRequestDto)
        {
            var itemType =
                new Model.ItemType(
                    id: Guid.NewGuid(),
                    code: postRequestDto.Code);

            await _itemTypeRepository.SaveAsync(itemType);

            var dto = MapToDto(ItemTypeReadModel.Map(itemType));

            var url = Url.Action(nameof(Get), new { id = itemType.Id });

            return Created(url, dto);
        }

        [HttpGet]
        [Route("")]
        public async Task<ObjectResult> GetByItemCode([FromQuery]string code)
        {
            var entity = await _readRepository.GetAsync(code);
            if (entity == null)
            {
                return NotFound(code);
            }

            return Ok(MapToDto(entity));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ObjectResult> Get([FromRoute] Guid id)
        {
            var entity = await _itemTypeRepository.GetAsync(id);

            var dto = MapToDto(ItemTypeReadModel.Map(entity));

            return Ok(dto);
        }

        private static ItemTypeDto MapToDto(Model.ItemTypeReadModel itemType)
        {
            var dto = new ItemTypeDto
            {
                Id = itemType.Id,
                Code = itemType.Code
            };
            return dto;
        }
    }
}