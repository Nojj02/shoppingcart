using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartHandlers.Tests.TestHelpers;
using ShoppingCartHandlers.Tests.TestHelpers.Web;
using ShoppingCartHandlers.Web;
using Xunit;

namespace ShoppingCartHandlers.Tests.Web
{
    public class EventApiTests
    {
        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasNoContent()
        {
            var httpClientWrapper = new MockEventHttpClientWrapper(null);

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, EventConverter.Empty, 10);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-9", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        
        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseContentIsEmptyString()
        {
            var httpClientWrapper = new MockEventHttpClientWrapper(String.Empty);

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, EventConverter.Empty, 10);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-9", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasOneEvent()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "test-message-type",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                10);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-9", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.NotNull(result);
            Assert.IsType<TestResourceCreatedEvent>(result[0]);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceCreatedEvent) result[0]).Id);
        }

        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasMultipleEvents()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "test-message-type",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        },

                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                10);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-9", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.Equal(3, result.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceCreatedEvent) result[0]).Id);
            Assert.Equal(new Guid("9435310E-098B-4598-A378-5862D9A9E9AE"), ((TestResourceCreatedEvent) result[1]).Id);
            Assert.Equal(new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA"), ((TestResourceCreatedEvent) result[2]).Id);
        }

        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasDifferentTypesOfEvents()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "resource",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-updated",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                10);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-9", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.Equal(3, result.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceCreatedEvent) result[0]).Id);
            Assert.Equal(new Guid("9435310E-098B-4598-A378-5862D9A9E9AE"), ((TestResourceCreatedEvent) result[1]).Id);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceUpdatedEvent) result[2]).Id);
        }

        [Fact]
        public async Task MakesAnExtraCall_NumberOfEventsExactSizeAsOneBatch()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "resource",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-updated",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                3);

            var result = await apiHelper.GetEventsAfterAsync("resource", lastMessageNumber: MessageNumber.NotSet);

            Assert.Equal(2, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/0-2", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);
            Assert.Equal("/events/resource/3-5", httpClientWrapper.MessagesSent[1].RequestUri.AbsolutePath);

            Assert.Equal(3, result.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceCreatedEvent) result[0]).Id);
            Assert.Equal(new Guid("9435310E-098B-4598-A378-5862D9A9E9AE"), ((TestResourceCreatedEvent) result[1]).Id);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceUpdatedEvent) result[2]).Id);
        }


        [Fact]
        public async Task CallsSecondBatchOfEvents_LastMessageNumberIsHigherThanOneBatchSize()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "test-message-type",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("093626F4-7125-41CD-9982-785B7D52BCAA")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("C911BCDB-F23C-4FA3-BE57-E2C0EC3793DC")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                3);

            var result = await apiHelper.GetEventsAfterAsync(resourceName: "resource", lastMessageNumber: MessageNumber.New(3));

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/3-5", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.Equal(1, result.Count);
            Assert.Equal(new Guid("C911BCDB-F23C-4FA3-BE57-E2C0EC3793DC"), ((TestResourceCreatedEvent) result[0]).Id);
        }

        [Fact]
        public async Task MultipleCallsUntilAllEventsAreRetrieved_EventsCoverMultipleBatches()
        {
            var httpClientWrapper =
                new FakeEventHttpClientWrapper(
                    resourceName: "test-message-type",
                    events: new List<EventInfo>
                    {
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("093626F4-7125-41CD-9982-785B7D52BCAA")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("C911BCDB-F23C-4FA3-BE57-E2C0EC3793DC")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("00DE8FA8-98E7-4E65-8CE0-B3C982BD1B03")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("F2E3B971-28C9-474E-9EFA-231C86A673B4")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("76027085-3B4C-4724-A88C-375E5AB24E7A")
                                }
                        },
                        new EventInfo
                        {
                            EventType = "resource-created",
                            Event =
                                new TestResourceCreatedEvent
                                {
                                    Id = new Guid("E34AD640-B4D0-4A31-9A19-7F1B869937C2")
                                }
                        }
                    });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, TestResourceEventConverter.Instance,
                3);

            var result = await apiHelper.GetEventsAfterAsync(resourceName: "resource", lastMessageNumber: MessageNumber.New(3));

            Assert.Equal(3, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/events/resource/3-5", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);
            Assert.Equal("/events/resource/6-8", httpClientWrapper.MessagesSent[1].RequestUri.AbsolutePath);
            Assert.Equal("/events/resource/9-11", httpClientWrapper.MessagesSent[2].RequestUri.AbsolutePath);

            Assert.Equal(5, result.Count);
            Assert.Equal(new Guid("C911BCDB-F23C-4FA3-BE57-E2C0EC3793DC"), ((TestResourceCreatedEvent) result[0]).Id);
            Assert.Equal(new Guid("00DE8FA8-98E7-4E65-8CE0-B3C982BD1B03"), ((TestResourceCreatedEvent) result[1]).Id);
            Assert.Equal(new Guid("F2E3B971-28C9-474E-9EFA-231C86A673B4"), ((TestResourceCreatedEvent) result[2]).Id);
            Assert.Equal(new Guid("76027085-3B4C-4724-A88C-375E5AB24E7A"), ((TestResourceCreatedEvent) result[3]).Id);
            Assert.Equal(new Guid("E34AD640-B4D0-4A31-9A19-7F1B869937C2"), ((TestResourceCreatedEvent) result[4]).Id);
        }
    }
}