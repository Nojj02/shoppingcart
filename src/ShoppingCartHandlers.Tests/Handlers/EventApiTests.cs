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
        public async Task CallsFirstBatchOfEvents()
        {
            var recordUrlsHttpClientWrapper = new RecordHttpRequestMessagesHttpClientWrapper();

            var apiHelper = new EventApi("http://localhost/", recordUrlsHttpClientWrapper, EventConverter.Empty);

            await apiHelper.GetNewEventsAsync("resource");

            Assert.Equal(1, recordUrlsHttpClientWrapper.MessagesSent.Count);

            Assert.Equal("/resource/0-10", recordUrlsHttpClientWrapper.MessagesSent[0].RequestUri.AbsolutePath);
        }

        [Fact]
        public async Task MapsEventToProperSignature()
        {
            var message = new
            {
                messageType = "test-message-type",
                events = new List<TestResourceEvent>
                {
                    new TestResourceEvent
                    {
                        Id = new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0")
                    }
                }
            };

            var recordUrlsHttpClientWrapper = new FakeHttpClientWrapper(JsonConvert.SerializeObject(message));

            var eventConverter = new EventConverter(new Dictionary<string, Type>
            {
                { "test-message-type", typeof(TestResourceEvent) }
            });

            var apiHelper = new EventApi("http://localhost/", recordUrlsHttpClientWrapper, eventConverter);

            var result = await apiHelper.GetNewEventsAsync("resource");

            Assert.NotNull(result);
            Assert.IsType<TestResourceEvent>(result[0]);
            Assert.Equal(new Guid("7a60d915-a25a-4678-b25e-e35a45a2f0c0"), ((TestResourceEvent)result[0]).Id);
        }
    }
}