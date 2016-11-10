#define CONTRACTS_FULL
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Context;
using RawRabbit.Context.Enhancer;
using RawRabbit.Exceptions;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public class RabbitWorker:IWorker
    {
        protected IBusClient<AdvancedMessageContext> Client { get; set; }

        protected WorkerInstanceConfiguration Configuration { get; set; }

        protected virtual void Initialize()
        {
            
        }

        public void Init(IBusClient<AdvancedMessageContext> client, WorkerInstanceConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            Client = client;
            Configuration = configuration;
            Initialize();
        }

        /// <summary>
        /// Method will publish to RabbitMQ. If message is delivered within 
        /// PublishConfirmTimeout true will be returned, otherwise false will be returned.
        /// </summary>
        /// <param name="endpointName">Used to located the named endpoint from the configuration.</param>
        /// <param name="msg">The message that is to be sent.</param>
        /// <param name="setRoutingKey">Function allows routing key to be modified. IN parameter is routing key from config.</param>
        /// <param name="messageId">The messageID assigned to this message.</param>
        /// <returns>True if publish is sucessful, false if it fails.</returns>
        public async Task<bool> Publish<T>(string endpointName, T msg, Func<string,string> setRoutingKey = null, Guid messageId = default(Guid))
        {
            messageId = Guid.NewGuid();

            var cfg = Configuration.PublishToConfig.First(e => e.Name == endpointName);

            if (cfg == null)
            {
                throw new Exception($"Unable to find endpoint named {endpointName}");
            }

            string routingKey = string.Empty;
            if (setRoutingKey != null)
            {
                //run function to modify routing.
                routingKey = setRoutingKey(cfg.RoutingKey);
            }

            if(string.IsNullOrEmpty(routingKey))
            { 
                routingKey = cfg.RoutingKey;
            }


            try
            {
                //publish message. Use configuration 
                await Client.PublishAsync(msg, messageId, config =>
                {
                    config.WithExchange(e =>

                        e.WithType(cfg.ExchangeType)
                            .WithName(cfg.ExchangeName));


                    if (!string.IsNullOrEmpty(routingKey))
                    {
                        config.WithRoutingKey(routingKey);
                    }

                });

            }
            //Exception happens if publish timeout is exceeded before acknowlege.
            catch (PublishConfirmException)
            {
                return false;
            }

            return true;
        }

        public async Task<TResponse> Request<TRequest, TResponse>(string endpointName, TRequest msg,
            Func<string, string> setRoutingKey = null, Guid messageId = default(Guid))
        {
            
            if (Configuration.PublishToConfig == null)
            {
                throw new Exception("PublishToConfig is required.");
            }

            var cfg = Configuration.PublishToConfig.First(e => e.Name == endpointName);

            if (cfg == null)
            {
                throw new Exception($"Unable to find endpoint named {endpointName}");
            }

            string routingKey = string.Empty;
            if (setRoutingKey != null)
            {
                //run function to modify routing.
                routingKey = setRoutingKey(cfg.RoutingKey);
            }

            if (string.IsNullOrEmpty(routingKey))
            {
                routingKey = cfg.RoutingKey;
            }
            
            //publish message. Use configuration 
            return await Client.RequestAsync<TRequest,TResponse>(msg, messageId, config =>
            {
                config.WithExchange(e =>
                        e.WithType(cfg.ExchangeType)
                            .WithName(cfg.ExchangeName))
                    .WithRoutingKey(routingKey);
            });

           
            
        }
    }
}