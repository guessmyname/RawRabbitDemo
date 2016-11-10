using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;
using  exchange=RawRabbit.Configuration.Exchange;

namespace SET.IR.Worker.Core
{
    public abstract class SubscribeWorker<T>:RabbitWorker where T:class,IMessage
    {

       
        protected override void Initialize()
        {
            var cfg = Configuration.SubsciptionConfig;

            if (cfg == null)
            {
                throw new Exception("SubsciptionConfig is required.");
            }

            if (cfg.RoutingKeys == null || cfg.RoutingKeys.Count < 1)
            {
                SetSubscription(cfg, "#");
            }
            else
            {

                foreach (var routingKey in cfg.RoutingKeys)
                {

                    SetSubscription(cfg, routingKey);
                }
            }
        }

        private  void SetSubscription(SubsciptionConfig cfg, string routingKey)
        {
            Client.SubscribeAsync<T>(async (msg, context) =>
            {
                await HandleMessage(msg, context.RetryInfo.NumberOfRetries, context.RetryLater)
                    .ContinueWith(t =>
                    {
                        if (context.RetryInfo.NumberOfRetries < cfg.AllowedRetries)
                        {
                            context.RetryLater(TimeSpan.FromSeconds(120));
                            var ignored = t.Exception;
                        }
                        else
                        {
                            if (t.Exception != null) throw t.Exception;
                        }
                    }, TaskContinuationOptions.OnlyOnFaulted);
            }, config =>
            {
                config.WithExchange(exchange =>
                {
                    exchange.WithName(cfg.ExchangeName);
                    exchange.WithType(cfg.ExchangeType);
                });

                if (string.IsNullOrEmpty(routingKey))
                {
                    config.WithRoutingKey(routingKey);
                }
                
            });
        }


        protected abstract Task HandleMessage(T message, long retryCount, Action<TimeSpan> retryLater);
        
    }
}