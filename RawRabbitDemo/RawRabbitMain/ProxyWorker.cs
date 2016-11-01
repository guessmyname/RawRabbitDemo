using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Common;
using RawRabbit.Configuration.Publish;
using RawRabbit.Configuration.Subscribe;
using RawRabbit.Context;
using RawRabbit.ErrorHandling;
using RawRabbit.Operations;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace RawRabbitMain
{
   public abstract class ProxyWorker
   {
       private readonly IBusClient _client;

       protected ProxyWorker()
       {
            _client = BusClientFactory.CreateDefault(config =>
            {
                config.AddTransient<IErrorHandlingStrategy, CustomErrorHandling>();
            });
        }

       public delegate void MessagePublished();

       public event MessagePublished MessagePublishedEvent;

        public delegate void MessageProcessed();

        public event MessageProcessed MessageProcessedEvent;

        public ISubscription SubscribeAsync<T>(Func<T,MessageContext,Task> subscribeMethod, Action<ISubscriptionConfigurationBuilder> configuration = null  )
        {
            var subscribeWrapper = new Func<T, MessageContext, Task>((arg1, context) =>
            {

                var subscribeTask = subscribeMethod(arg1, context);

                subscribeTask.ContinueWith(task =>
                {
                    if (task.IsFaulted == false)
                    {
                        MessageProcessedEvent?.Invoke();
                    }
                });

                return subscribeTask;

            });
            
           return _client.SubscribeAsync(subscribeWrapper, configuration);
        }

       public Task PublishAsync<T>(T message, Guid messageId = default(Guid),Action<IPublishConfigurationBuilder> configuration = null)
       {
            MessagePublishedEvent?.Invoke();

            return _client.PublishAsync(message, messageId, configuration);

       }
    }
}
