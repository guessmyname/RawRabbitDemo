using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Context;
using RawRabbit.vNext;

namespace SET.IR.Worker.Core.Tests
{
    [TestClass]
    public class SubscriberWorkerTest
    {

        
        [TestMethod]
        public void TestMethod1()
        {
            var finished = new ManualResetEventSlim(false);

            var testMessage = "Test";

            string results = string.Empty;

            var client = BusClientFactory.CreateDefault<AdvancedMessageContext>();

            var pubcfg = new PublishConfig()
            {
                Name = "SubscribeTest",
                ExchangeName = "SubscribeTestResponse",
                ExchangeType = ExchangeType.Topic
            };
            var config = new WorkerInstanceConfiguration()
            {
                SubsciptionConfig = new SubsciptionConfig()
                {
                    ExchangeName = "SubscribeTest",
                    ExchangeType = ExchangeType.Topic
                },
                PublishToConfig = new List<PublishConfig>
                {
                    pubcfg
                }
            };
            
            var testobj = new TestSubscribeWorker( message =>
            {
                results = message.Message;
                finished.Set();
            }
            ) {Configuration = config};
            testobj.Init();

            client.PublishAsync(new TestMessage() {Message = testMessage}, Guid.NewGuid(),
                c =>
                    c.WithExchange(
                        e =>
                            e.WithName(config.SubsciptionConfig.ExchangeName)
                                .WithType(config.SubsciptionConfig.ExchangeType)));

            Assert.IsTrue(finished.Wait(2000));
            
            Assert.AreEqual(testMessage,results);

        }
    }
}
