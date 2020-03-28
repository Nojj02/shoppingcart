using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Events;
using ShoppingCartEvents;

namespace ShoppingCartApi.Controllers.Events
{
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICouponRepository _couponRepository;

        public EventsController(IItemTypeRepository itemTypeRepository,
            IItemRepository itemRepository,
            ICouponRepository couponRepository)
        {
            _itemTypeRepository = itemTypeRepository;
            _itemRepository = itemRepository;
            _couponRepository = couponRepository;
        }
        
        [HttpGet]
        [Route("itemType/{startIndex}-{endIndex}")]
        public async Task<ObjectResult> GetItemTypeEvents(int startIndex, int endIndex)
        {
            var events = await _itemTypeRepository.GetEventsAsync(startIndex, endIndex);
            
            var transportMessage =
                MapEventsToTransportMessage("itemType", events);

            return Ok(transportMessage);
        }

        [HttpGet]
        [Route("item/{startIndex}-{endIndex}")]
        public async Task<ObjectResult> GetItemEvents(int startIndex, int endIndex)
        {
            var events = await _itemRepository.GetEventsAsync(startIndex, endIndex);

            var transportMessage =
                MapEventsToTransportMessage("item", events);

            return Ok(transportMessage);
        }
        
        [HttpGet]
        [Route("coupon/{startIndex}-{endIndex}")]
        public async Task<ObjectResult> GetCouponEvents(int startIndex, int endIndex)
        {
            var events = await _couponRepository.GetEventsAsync(startIndex, endIndex);

            var transportMessage =
                MapEventsToTransportMessage("coupon", events);

            return Ok(transportMessage);
        }
        private static TransportMessage MapEventsToTransportMessage(
            string messageType,
            IReadOnlyList<IEvent> events)
        {
            return new TransportMessage
            {
                Events = events
                    .Select(x => new EventInfo
                    {
                        EventType = ShoppingCartEventConverter.Instance.GetTypeOf(x.GetType()),
                        Event = x
                    }).ToList(),
                MessageType = messageType
            };
        }
    }
}