using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ShoppingCartHandlers.Tests.Handlers
{
    public class EventApiTests
    {
        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasNoContent()
        {
            var httpClientWrapper = new FakeEventHttpClientWrapper();

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, EventConverter.Empty);

            var result = await apiHelper.GetNewEventsAsync("resource");

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/resource/0-10", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);
            
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasOneEvent()
        {
            var httpClientWrapper = 
                new FakeEventHttpClientWrapper(
                    messageType: "test-message-type",
                    events: new List<TestResourceEvent>
                    {
                        new TestResourceEvent
                        {
                            Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                        }
                    });

            var eventConverter = new EventConverter(new Dictionary<string, Type>
            {
                { "test-message-type", typeof(TestResourceEvent) }
            });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, eventConverter);

            var result = await apiHelper.GetNewEventsAsync("resource");

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/resource/0-10", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.NotNull(result);
            Assert.IsType<TestResourceEvent>(result[0]);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceEvent)result[0]).Id);
        }

        [Fact]
        public async Task CallsFirstBatchOfEvents_ResponseHasMultipleEvents()
        {
            var httpClientWrapper = 
                new FakeEventHttpClientWrapper(
                    messageType: "test-message-type",
                    events: new List<TestResourceEvent>
                    {
                        new TestResourceEvent
                        {
                            Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                        },
                        new TestResourceEvent
                        {
                            Id = new Guid("9435310E-098B-4598-A378-5862D9A9E9AE")
                        },
                        new TestResourceEvent
                        {
                            Id = new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA")
                        }
                    });

            var eventConverter = new EventConverter(new Dictionary<string, Type>
            {
                { "test-message-type", typeof(TestResourceEvent) }
            });

            var apiHelper = new EventApi("http://localhost/", httpClientWrapper, eventConverter);

            var result = await apiHelper.GetNewEventsAsync("resource");

            Assert.Equal(1, httpClientWrapper.MessagesSent.Count);
            Assert.Equal("/resource/0-10", httpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);

            Assert.Equal(3, result.Count);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceEvent)result[0]).Id);
            Assert.Equal(new Guid("9435310E-098B-4598-A378-5862D9A9E9AE"), ((TestResourceEvent)result[1]).Id);
            Assert.Equal(new Guid("73DE4B51-1F0E-4428-8D59-8DFBEA8091BA"), ((TestResourceEvent)result[2]).Id);
        }
    }
}