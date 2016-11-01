using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.ErrorHandling;
using RawRabbit.Logging;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace RawRabbitMain
{
   public class ProxySystem
    {
       private IBusClient _client;

        private Dictionary<int,RabbitWatch> wattchedRabbits = new Dictionary<int, RabbitWatch>();
       public ProxySystem(IServiceCollection serviceCollection)
       {
           var services = serviceCollection.BuildServiceProvider();

            
            _client = BusClientFactory.CreateDefault(config =>
            {
                config.AddTransient<IErrorHandlingStrategy, CustomErrorHandling>();
            });
            

          
           
           _client.SubscribeAsync<CommandMessage>(async (cmd, context) =>
           {

               await Task.Yield();
               switch (cmd.Name)
               {
                   case "Create":

                       var rabbit =new RabbitWatch("IRProxy",$"Rabbit{wattchedRabbits.Count+1}",true,30000);
                       wattchedRabbits.Add(wattchedRabbits.Count+1, rabbit);
                       break;

                   case "Destroy":


                       var rab = wattchedRabbits.First();
                       wattchedRabbits.Remove(rab.Key);
                       rab.Value.Dispose();
                       
                       break;


                   case "Count":

                      var logger = LogManager.GetLogger<ConsoleLogger>();

                       logger.LogInformation("Total rabbits {0}", wattchedRabbits.Count);
                       break;

                   case "Fail":
                       throw new ApplicationException();
                       
               }

               
           }, config =>
           {
               config.WithExchange(exchange =>
               {
                   exchange.WithAutoDelete(false).WithName("SystemCommands");
               });
           });

       }
    }

    public class CommandMessage
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
