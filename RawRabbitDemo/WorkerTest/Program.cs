using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Context;
using RawRabbit.ErrorHandling;
using Microsoft.Extensions.Configuration.Json;
using RawRabbit.Extensions.Disposable;
using SET.IR.Worker.Core;
using RawRabbit.vNext;

namespace WorkerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var client =
                BusClientFactory.CreateDefault<AdvancedMessageContext>(
                    config => config.AddJsonFile("RabbitConfig.json"), null);

             var service = new ServiceCollection();
            
            RegisterDependencies(client,service);
            
         var provider =  service.BuildServiceProvider();

           
            var cfg1 = JsonConvert.DeserializeObject<WorkerHostConfiguration>(myconfig);


            var host = new WorkerHost(provider, cfg1);

              
            var conventions = provider.GetService<INamingConventions>();

           
            client.SubscribeAsync<HandlerExceptionMessage>(async (msg, context) =>
            {
                context.RetryLater(TimeSpan.FromSeconds(200));
                await Console.Out.WriteLineAsync(msg.Message.ToString());
            }, c => c
                .WithExchange(e =>
                {
                    e.WithName(conventions.ErrorExchangeNamingConvention())
                    .WithDurability(false)
                    .WithType(ExchangeType.Topic);
                })
                .WithRoutingKey("#"));


            var testMsg = new TestMessage { Message = "Test" };


            while (true)
            {
                client.PublishAsync(testMsg, Guid.NewGuid(), config =>
                {
                    config.WithRoutingKey("Test")
                        .WithExchange(e => e.WithName("Test")
                            .WithType(ExchangeType.Topic));
                });

                var line = Console.ReadLine();
            }
        }



        private static void RegisterDependencies(RawRabbit.vNext.Disposable.IBusClient<AdvancedMessageContext> client, IServiceCollection service)
        {
            service.AddSingleton(client);
            service.AddTransient<TestSubscribeWorker>();
           
        }


        private static string myconfig = @"{
	'SystemName':'EventProcessor',	
	WorkerConfigurations:[
	{
		'Type':'WorkerTest.TestSubscribeWorker, Workers',
		'NumberOfInstances':2,	
		'InstanceConfiguration':{
		
			'ExchangeName':'Xyz',
			'ExchangeType':'Topic',
			'RoutingKeys':['Test'],
			'AllowedRetries':3,
			'PublishToConfig':[{
				'ExchangeName':'ABC',
				'RoutingKey':'KeyYZ',
				'ExchangeType':'Direct'
			}],
            'CustomSettings':
              {
                'Test':'123'
                }
                
		}
	}
]

}";
    }
}
