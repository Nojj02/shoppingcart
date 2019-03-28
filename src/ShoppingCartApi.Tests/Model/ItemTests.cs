using System;
using System.Collections.Generic;
using ShoppingCartApi.Model;
using ShoppingCartApi.Model.Events;
using Xunit;

namespace ShoppingCartApi.Tests.Model
{
    public class ItemTests
    {
        public class Constructor : ItemTests
        {
            [Fact]
            public void GeneratesEventAndPropertiesSet()
            {
                var item = 
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), 
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));
                
                Assert.Single(item.Events);
                Assert.Single(item.NewEvents);

                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), item.Id);
                Assert.Equal("banana", item.Code);
                Assert.Equal(50m, item.Price);
                Assert.Equal(new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"), item.ItemTypeId);

                var anEvent = item.Events[0] as ItemCreatedEvent;
                Assert.NotNull(anEvent);
                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), anEvent.Id);
                Assert.Equal(1, anEvent.Version);
                Assert.Equal("banana", anEvent.Code);
                Assert.Equal(50m, anEvent.Price);
                Assert.Equal(new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"), anEvent.ItemTypeId);
            }
        }

        public class SetPercentageDiscount : ItemTests
        {
            [Fact]
            public void GeneratesEventAndPropertiesSet()
            {
                var item =
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"),
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));

                item.SetPercentageDiscount(new Percent(50));
                Assert.Equal(new Percent(50), item.PercentOff);

                Assert.Equal(2, item.Events.Count);
                Assert.Equal(2, item.NewEvents.Count);

                var anEvent = item.Events[1] as ItemPercentageDiscountSetEvent;
                Assert.NotNull(anEvent);
                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), anEvent.Id);
                Assert.Equal(2, anEvent.Version);
                Assert.Equal(new Percent(50), anEvent.PercentOff);
            }
        }

        public class SetAmountDiscount : ItemTests
        {
            [Fact]
            public void GeneratesEventAndPropertiesSet()
            {
                var item =
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"),
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));

                item.SetAmountDiscount(30);
                Assert.Equal(30m, item.AmountOff);

                Assert.Equal(2, item.Events.Count);
                Assert.Equal(2, item.NewEvents.Count);

                var anEvent = item.Events[1] as ItemAmountDiscountSetEvent;
                Assert.NotNull(anEvent);
                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), anEvent.Id);
                Assert.Equal(2, anEvent.Version);
                Assert.Equal(30m, anEvent.AmountOff);
            }
        }

        public class Reconstitute : ItemTests
        {
            [Fact]
            public void RebuildsFromEvents()
            {
                var item =
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"),
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));

                item.SetAmountDiscount(30);
                item.SetPercentageDiscount(new Percent(70));
                
                var reconstitutedItem = new Item(item.Id, item.Events);

                Assert.Equal(3, reconstitutedItem.Events.Count);

                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), reconstitutedItem.Id);
                Assert.Equal("banana", reconstitutedItem.Code);
                Assert.Equal(50m, reconstitutedItem.Price);
                Assert.Equal(new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"), reconstitutedItem.ItemTypeId);
                Assert.Equal(30, reconstitutedItem.AmountOff);
                Assert.Equal(new Percent(70), reconstitutedItem.PercentOff);
            }

            [Fact]
            public void NewEventsFromReconstitutedEntitiesAreStoredAsNewEvents()
            {
                var item =
                    new Item(
                        id: new Guid("f61f0355-796e-4f56-963c-734ac2e52121"),
                        code: "banana",
                        price: 50m,
                        itemTypeId: new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"));

                item.SetAmountDiscount(30);
                item.SetPercentageDiscount(new Percent(70));

                var reconstitutedItem = new Item(item.Id, item.Events);
                reconstitutedItem.SetAmountDiscount(40);

                Assert.Equal(4, reconstitutedItem.Events.Count);
                Assert.Equal(1, reconstitutedItem.NewEvents.Count);

                Assert.Equal(new Guid("f61f0355-796e-4f56-963c-734ac2e52121"), reconstitutedItem.Id);
                Assert.Equal("banana", reconstitutedItem.Code);
                Assert.Equal(50m, reconstitutedItem.Price);
                Assert.Equal(new Guid("c8867796-5cc5-49b2-ac9f-e559085d6b04"), reconstitutedItem.ItemTypeId);
                Assert.Equal(40, reconstitutedItem.AmountOff);
                Assert.Equal(new Percent(70), reconstitutedItem.PercentOff);
            }
        }
    }
}