using System;
using System.Linq;
using System.Threading.Tasks;
using RawRabbit.Configuration.Publish;
using RawRabbit.Context;
using RawRabbit.Exceptions;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public abstract class PublishWorker:Worker, IPublishWorker
    {
       protected PublishWorker(IBusClient<AdvancedMessageContext> client) : base(client)
       {
       }


        /// <summary>
        /// Method will publish to RabbitMQ. If message is delivered within 
        /// PublishConfirmTimeout true will be returned, otherwise false will be returned.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="messageId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<bool> Publish(object msg, Guid messageId = default(Guid), Action<IPublishConfigurationBuilder> config = null)
        {
            try
            {
                await Client.PublishAsync(msg, messageId, config);

            }
            catch (PublishConfirmException)
            {
                return false;
            }

            return true;


        }

        public async Task<bool> Publish(string endpointName, object msg, Guid messageId = default(Guid))
        {
            var cfg = Configuration.PublishToConfig.First(e => e.Name == endpointName);

            if (cfg == null)
            {
                throw new Exception($"Unable to find endpoint named {endpointName}");
            }

            try
            {
                await Client.PublishAsync(msg, messageId, config =>
                {
                    config.WithExchange(e => 
                    e.WithType(cfg.ExchangeType)
                    .WithName(cfg.ExchangeName))
                    .WithRoutingKey(cfg.RoutingKey);
                });

            }
            catch (PublishConfirmException)
            {
                return false;
            }

            return true;
        }

    }
}
