using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;
using ShoppingCartApi.Utilities.CustomActionResults;
using ShoppingCartReader.DataAccess;
using ShoppingCartReader.Model;
using ShoppingCartSharedKernel;

namespace ShoppingCartApi.Controllers.Item
{
    [Route("items")]
    public class ItemController : Controller
    {
        private readonly IItemRepository _repository;
        private readonly IItemReadRepository _readRepository;

        public ItemController(IItemRepository repository, IItemReadRepository readRepository)
        {
            _repository = repository;
            _readRepository = readRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<ObjectResult> Post([FromBody]PostRequestDto postRequestDto)
        {
            var existingItem = await _readRepository.GetAsync(postRequestDto.Code);
            if (existingItem != null)
            {
                var selfUrl = Url.Action("GetByItemCode", new { code = existingItem.Code });
               
                return new SeeOtherObjectResult(selfUrl, MapToDto(existingItem));
            }
            else
            {
                var entity = new Model.Item(
                    id: Guid.NewGuid(),
                    code: postRequestDto.Code,
                    price: postRequestDto.Price,
                    itemTypeId: postRequestDto.ItemTypeId);
                await _repository.SaveAsync(entity, DateTimeOffset.UtcNow);

                var selfUrl = Url.Action("GetByItemCode", new { code = entity.Code });
                return Created(selfUrl, MapToDto(entity));
            }
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

        [HttpPost]
        [Route("{id}/setDiscount")]
        public async Task<ObjectResult> SetDiscount(Guid id, [FromBody]SetDiscountRequestDto setDiscountRequestDto)
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null)
            {
                return NotFound(id);
            }

            entity.SetPercentageDiscount(new Percent(setDiscountRequestDto.PercentOff));
            entity.SetAmountDiscount(setDiscountRequestDto.AmountOff);
            await _repository.UpdateAsync(entity, DateTimeOffset.UtcNow);

            return Ok(MapToDto(entity));
        }

        private static ItemDto MapToDto(Model.Item entity)
        {
            return new ItemDto
            {
                Id = entity.Id,
                Code = entity.Code,
                ItemTypeId = entity.ItemTypeId,
                Price = entity.Price,
                PercentOff = entity.PercentOff.Value,
                AmountOff = entity.AmountOff
            };
        }

        private static ItemDto MapToDto(ItemReadModel entity)
        {
            return new ItemDto
            {
                Id = entity.Id,
                Code = entity.Code,
                ItemTypeId = entity.ItemTypeId,
                Price = entity.Price,
                PercentOff = entity.PercentOff.Value,
                AmountOff = entity.AmountOff
            };
        }
    }
}