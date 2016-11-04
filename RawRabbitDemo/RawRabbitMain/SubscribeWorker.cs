using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Configuration.Publish;
using RawRabbit.Context.Enhancer;
using RawRabbit.Exceptions;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace RawRabbitMain
{
    public abstract class SubscribeWorker<T>:Worker where T:class,IMessage
    {
        protected SubscribeWorker(WorkerInstanceConfiguration configuration) : base(configuration)
        {
            Initialize();
        }
        private void Initialize()
        {
           
            foreach (var routingKey in Configuration.RoutingKeys)
            {
                ExchangeType exchangeType;

                switch (Configuration.ExchangeType)
                {
                    case "Direct":

                        exchangeType = ExchangeType.Direct;
                        break;
                        
                    default:
                        exchangeType = ExchangeType.Topic;
                        break;
                }
                Client.SubscribeAsync<T>(async (msg, context) =>
                {
                    await HandleMessage(msg, context.RetryInfo.NumberOfRetries, context.RetryLater)
                    .ContinueWith(t =>
                    {

                        if (context.RetryInfo.NumberOfRetries < Configuration.AllowedRetries)
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
                        exchange.WithName(Configuration.ExchangeName);
                        exchange.WithType(exchangeType);
                    });
                    config.WithRoutingKey(routingKey);
                });
            }
           
        }


        protected abstract Task HandleMessage(T message, long retryCount, Action<TimeSpan> retryLater);
        
    }


    public abstract class Worker:IWorker
    {
        protected readonly WorkerInstanceConfiguration Configuration;
        protected readonly IBusClient<DetailedContext> Client;
        protected Worker(WorkerInstanceConfiguration configuration)
        {
            Configuration = configuration;

            var cfg = RawRabbitConfiguration.Local.AsLegacy();
            Client = BusClientFactory.CreateDefault<DetailedContext>(null, config =>
            {

                config.AddSingleton<IContextEnhancer, DetailedContextEnhancer>();
                config.AddSingleton(s => cfg);

            });

        }
    }
    public interface IWorker
    {
        
    }
}