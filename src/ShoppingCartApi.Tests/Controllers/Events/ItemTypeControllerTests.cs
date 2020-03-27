using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartApi.Controllers.Events;
using ShoppingCartApi.DataAccess;
using ShoppingCartApi.Events;
using ShoppingCartEvents;
using Xunit;

namespace ShoppingCartApi.Tests.Controllers.Events
{
    public class ItemTypeControllerTests
    {
        [Fact]
        public async Task Test()
        {
            var itemType = 
                new ShoppingCartApi.Model.ItemType(
                    id: new Guid("FC9AAB00-A6BA-4EE1-8DD3-30427C9E01A2"), 
                    code: "fruits");
            
            var itemTypeRepository = new InMemoryItemTypeRepository();
            await itemTypeRepository.SaveAsync(itemType, DateTimeOffset.UtcNow);
            var controller = new ItemTypeController(itemTypeRepository);

            var result = await controller.Get(0, 5);
            var transportMessage = (TransportMessage)result.Value;
            
            Assert.Equal("itemtype", transportMessage.MessageType);
            Assert.Equal(1,transportMessage.Events.Count());
            var events = transportMessage.Events.ToList();
            
            Assert.Equal("itemtype-created", events[0].EventType);
            Assert.IsType<ItemTypeCreatedEvent>(events[0].Event);
        }
    }
}