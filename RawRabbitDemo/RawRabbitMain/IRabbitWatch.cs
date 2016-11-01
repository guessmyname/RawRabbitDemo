using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Context;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;
using Timer = System.Timers.Timer;

namespace RawRabbitMain
{
    interface IRabbitWatch
    {
    }


    public class RabbitWatch:IRabbitWatch,IDisposable
    {
        private Timer _timer;

        public RabbitWatch(string systemName, string rabbitName,bool enableHeartbeat = true, int heartbeatInteval = 5000)
        {
            _client = BusClientFactory.CreateDefault();

            
            _timer = new Timer();

            _timer.Elapsed += (state, args) =>
            {
                var processedCount = Interlocked.Read(ref _processedMessages);
                Interlocked.Exchange(ref _processedMessages, 0);

                var diag = new HeartbeatMessage
                {
                    MessagesProcessed = processedCount.ToString(),
                    RecordedTime = DateTime.Now
                };

                _client.PublishAsync(diag, Guid.NewGuid(), config =>
                {
                    config.WithExchange(exchange =>
                    {
                        exchange.WithAutoDelete(false)
                            .WithName("heartbeat")
                            .WithType(ExchangeType.Fanout);
                    })
                        .WithRoutingKey($"heartbeat.{systemName}.{rabbitName}");
                });
            };

            _timer.Interval = heartbeatInteval;
            _timer.Enabled = enableHeartbeat;

        }
        private long _processedMessages = 0;
        private readonly IBusClient _client;
        public event Action MessageReceived;
        public event Action MessageSent;


        public ISubscription Subscribe<T>(Func<T, MessageContext, Task> subscribeMethod)
        {
         
           return _client.SubscribeAsync<T>( async (msg, context) =>
            {
                MessageReceived?.Invoke();

                await subscribeMethod(msg, context);

                MessageSent?.Invoke();

                Interlocked.Increment(ref _processedMessages);
            });
            
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();

            _client.ShutdownAsync().Wait();

            _client.Dispose();
        }
    }
}
