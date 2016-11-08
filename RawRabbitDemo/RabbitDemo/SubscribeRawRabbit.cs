using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Configuration.Exchange;
using RawRabbit.vNext;

namespace RabbitDemo
{
   public class SubscribeRawRabbit
    {

        public void subscribe()
        {
            var client = BusClientFactory.CreateDefault();
            client.SubscribeAsync<string>(async (msg, context) =>
            {
                await Console.Out.WriteAsync($"Recieved: {msg}.");
            }, config =>
            {
                config.WithExchange(exchange => exchange.WithName("logs")
                .WithType(ExchangeType.Fanout));
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
