using System;
using System.Threading.Tasks;
using  exchange=RawRabbit.Configuration.Exchange;

namespace SET.IR.Worker.Core
{
    public abstract class SubscribeWorker<T>:PublishWorker where T:class,IMessage
    {

       
        protected SubscribeWorker(WorkerInstanceConfiguration configuration) : base(configuration)
        {
            Initialize();
        }
        private void Initialize()
        {
           
            foreach (var routingKey in Configuration.RoutingKeys)
            {
               
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
                        exchange.WithType(Configuration.ExchangeType);
                    });
                    config.WithRoutingKey(routingKey);
                });
            }
           
        }


        protected abstract Task HandleMessage(T message, long retryCount, Action<TimeSpan> retryLater);
        
    }
}