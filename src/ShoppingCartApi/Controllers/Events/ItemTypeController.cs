using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Events;

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
            var transportMessage =
                new TransportMessage
                {
                    Events = events
                        .Select(x => new EventInfo
                        {
                            EventType = "itemtype-created",
                            Event = x
                        }).ToList(),
                    MessageType = "itemtype"
                };

            return Ok(transportMessage);
        }
    }
}