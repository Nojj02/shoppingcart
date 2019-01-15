using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Api.Controllers.ItemType
{
    [Route("itemType")]
    public class ItemTypeController : Controller
    {
        private readonly List<string> _itemTypeCodes = new List<string>();

        [HttpPost]
        public async Task<ObjectResult> Post(PostNewItemTypeDto postNewItemTypeDto)
        {
            _itemTypeCodes.Add(postNewItemTypeDto.Code);
            return Created(Url.Action("GetByItemTypeCode"), new ItemTypeDto
            {
                Code = postNewItemTypeDto.Code
            });
        }

        [HttpGet]
        public async Task<ObjectResult> GetByItemTypeCode(string code)
        {
            var itemTypeCode = _itemTypeCodes.SingleOrDefault(x => x == code);
            if (itemTypeCode == null)
            {
                return NotFound(code);
            }

            return Ok(
                new ItemTypeDto
                {
                    Code = itemTypeCode
                });

            
        }
    }
}