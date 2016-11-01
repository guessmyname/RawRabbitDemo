using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.vNext;

namespace RawRabbitMain
{
    class Program
    {
        static void Main(string[] args)
        {

           

            //IServiceCollection serviceCollection = new ServiceCollection();

            //serviceCollection.AddTransient<RabbitWatch>();

            
            //ProxySystem proxySystem = new ProxySystem(serviceCollection);

            //var client = BusClientFactory.CreateDefault();



            //while (true)
            //{

            //    var line = Console.ReadLine();

            //    var cmd = new CommandMessage() { Name = line};

            //    client.PublishAsync(cmd, Guid.NewGuid(), builder =>
            //    {
            //        builder.WithExchange(exchnage =>
            //        {
            //            exchnage.WithName("SystemCommands").WithAutoDelete(false);
            //        });
            //    });
            //}
        }
    }
}
