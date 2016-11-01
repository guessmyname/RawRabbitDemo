using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Context;
using RawRabbit.ErrorHandling;
using RawRabbit.vNext;

namespace MongoTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {

            IServiceProvider service = null;

            var cfg = RawRabbitConfiguration.Local.AsLegacy();
            var client = BusClientFactory.CreateDefault<AdvancedMessageContext>(null,config =>
            {
                config.AddSingleton(s => cfg);
                service = config.BuildServiceProvider();

            });

            var conventions = service.GetService<INamingConventions>();


            client.SubscribeAsync<string>((msg, context) =>
            {
                throw new Exception(msg);
            }, config =>
            {
                config
                    .WithExchange(e => e.WithName("Events")
                        .WithType(ExchangeType.Direct)).WithRoutingKey("Event");
            });

            client.SubscribeAsync<HandlerExceptionMessage>(async (msg, context) =>
            {
                context.RetryLater( TimeSpan.FromSeconds(200));
                await Console.Out.WriteLineAsync(msg.Message.ToString());
            }, c => c
                .WithExchange(e => e.WithName(conventions.ErrorExchangeNamingConvention())
                .WithType(ExchangeType.Topic))
                .WithRoutingKey("#"));

            client.PublishAsync("test", Guid.NewGuid(), config =>
            {
                config.WithRoutingKey("Event")
                    .WithExchange(e => e.WithName("Events")
                        .WithType(ExchangeType.Direct));
            });

            while (true)
            {

                var line = Console.ReadLine();
            }
        }

        [TestMethod]
        public void TestMethod2()
        {

            IServiceProvider service = null;

            var cfg = RawRabbitConfiguration.Local.AsLegacy();
            var client = BusClientFactory.CreateDefault<AdvancedMessageContext>(null, config =>
            {
                config.AddSingleton(s => cfg);
                service = config.BuildServiceProvider();

            });

          
            client.SubscribeAsync<string>((msg, context) =>
            {
                throw new Exception(msg);
            }, config =>
            {
                config
                    .WithExchange(e => e.WithName("EventsTest")
                        .WithType(ExchangeType.Topic));
                
                config.WithRoutingKey("EventA");
            });

          

        }

    }
}
