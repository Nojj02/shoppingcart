using System;
using ShoppingCartApi.Model;
using Xunit;

namespace ShoppingCartApi.Tests.Model
{
    public class ItemTests
    {
        public class Constructor : ItemTests
        {
            [Fact]
            public void CreatedEvent()
            {
                var item = 
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), 
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));
                
                Assert.Single(item.Events);

                var createdEvent = item.Events[0] as ItemCreatedEvent;
                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), createdEvent.Id);
                Assert.Equal("banana", createdEvent.Code);
                Assert.Equal(50m, createdEvent.Price);
                Assert.Equal(new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"), createdEvent.ItemTypeId);
            }
        }
    }
}