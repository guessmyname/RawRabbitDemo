using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Context.Enhancer;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public abstract class Worker
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
}