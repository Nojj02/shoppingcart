using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;

namespace ShoppingCartApi.Controllers.Events
{
    [Route("events/itemType")]
    public class ItemTypeController : Controller
    {
        private readonly IItemTypeRepository _itemTypeRepository;

        public ItemTypeController(IItemTypeRepository itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
        }
        
        [HttpGet]
        [Route("{startIndex}-{endIndex}")]
        public async Task<ObjectResult> Get(int startIndex, int endIndex)
        {
            var events = await _itemTypeRepository.GetEventsAsync(startIndex, endIndex);

            return Ok(events);
        }
    }
}