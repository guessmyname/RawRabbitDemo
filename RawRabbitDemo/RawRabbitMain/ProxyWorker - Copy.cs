using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Configuration.Publish;
using RawRabbit.Configuration.Subscribe;
using RawRabbit.Context;
using RawRabbit.ErrorHandling;
using RawRabbit.Operations;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace RawRabbitMain
{
    public abstract class Worker<T> where T:class,IMessage
    {
        private readonly WorkerConfiguration _configuration;
        private readonly IBusClient<AdvancedMessageContext> _client;
        protected Worker(WorkerConfiguration configuration)
        {
            _configuration = configuration;
           
            var cfg = RawRabbitConfiguration.Local.AsLegacy();
            _client = BusClientFactory.CreateDefault<AdvancedMessageContext>(null,config =>
            {
              
                config.AddSingleton(s => cfg);

            });

            Initialize();
        }

        
        private void Initialize()
        {
            //var typename = typeof(T).Name;

            //_client.SubscribeAsync<HandlerExceptionMessage>(async (msg, context) =>
            //{
                
                
            //    await Console.Out.WriteLineAsync(msg.Message.ToString());
            //    if (ShouldRetry(msg.Exception, msg.Message as T))
            //    {
                    
            //    }
                
            //}, c => c
            //    .WithExchange(e => e.WithName(conventions.ErrorExchangeNamingConvention())
            //    .WithType(ExchangeType.Topic))
            //    .WithRoutingKey(typename));

            foreach (var routingKey in _configuration.RoutingKeys)
            {
                ExchangeType exchangeType;

                switch (_configuration.ExchangeType)
                {
                    case "Direct":

                        exchangeType = ExchangeType.Direct;
                        break;

                    default:
                        exchangeType = ExchangeType.Topic;
                        break;
                }
                _client.SubscribeAsync<T>(async (msg, context) =>
                {
                    await MessageReceived(msg, context.RetryInfo.NumberOfRetries, context.RetryLater).ContinueWith(t =>
                    {
                        
                        if (context.RetryInfo.NumberOfRetries < _configuration.Retries)
                        {
                            context.RetryLater(TimeSpan.FromSeconds(120));
                            var ignored = t.Exception;
                        }
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }, config =>
                {

                    config.WithExchange(exchange =>
                    {
                        exchange.WithName(_configuration.ExchangeName);
                        exchange.WithType(exchangeType);
                    });
                    config.WithRoutingKey(routingKey);
                });
            }
           
        }


        protected abstract Task MessageReceived(T message, long retryCount, Action<TimeSpan> retryLater);

    }

    
    public class WorkerConfiguration
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public List<string> RoutingKeys { get; set; }

        public int Retries { get; set; }
    }

    public interface IMessage
    {
        
    }
  }
