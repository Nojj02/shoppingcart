﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Model;
using ShoppingCartReader.DataAccess;

namespace ShoppingCartApi.Controllers.ItemType
{
    [Route("itemTypes")]
    public class ItemTypeController : ControllerBase
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

            await _itemTypeRepository.SaveAsync(itemType, DateTimeOffset.UtcNow);

            var dto = new ItemTypeDto
            {
                Id = itemType.Id,
                Code = itemType.Code
            };

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

            var dto = new ItemTypeDto
            {
                Id = entity.Id,
                Code = entity.Code
            };

            return Ok(dto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ObjectResult> Get([FromRoute] Guid id)
        {
            var entity = await _readRepository.GetAsync(id);

            var dto = new ItemTypeDto
            {
                Id = entity.Id,
                Code = entity.Code
            };

            return Ok(dto);
        }
    }
}