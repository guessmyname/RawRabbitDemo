using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;
using  exchange=RawRabbit.Configuration.Exchange;

namespace SET.IR.Worker.Core
{
    public abstract class SubscribeWorkerBase<T>:PublishWorker where T:class,IMessage
    {
        
        private readonly Dictionary<string, Func<T, long, Action<TimeSpan>,Task>> _messageHandlers = new Dictionary<string, Func<T, long, Action<TimeSpan>,Task>>();

        protected override void Initialize()
        {
            var cfg = Configuration.SubsciptionConfig;

            if (cfg == null)
            {
                throw new Exception("SubsciptionConfig is required.");
            }

            if (cfg.RoutingKeys == null || cfg.RoutingKeys.Count < 1)
            {
                SetSubscription(cfg, "#",_messageHandlers["#"]);
            }
            else
            {

                foreach (var routingKey in cfg.RoutingKeys)
                {
                    var handler = _messageHandlers[routingKey];
                    SetSubscription(cfg, routingKey, handler);
                }
            }
        }

        private  void SetSubscription(SubsciptionConfig cfg, string routingKey, Func<T, long, Action<TimeSpan>, Task> messageHandler)
        {
            Client.SubscribeAsync<T>(async (msg, context) =>
            {
                await messageHandler(msg, context.RetryInfo.NumberOfRetries, context.RetryLater)
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

        protected void AddMessageHandler(Func<T, long, Action<TimeSpan>,Task> messagehandler, string matchRoute = null )
        {
            if (matchRoute == null)
            {
                matchRoute = "#";
            }
            _messageHandlers.Add(matchRoute, messagehandler);
        }

    }
}