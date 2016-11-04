using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Context;
using RawRabbit.Context.Enhancer;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public abstract class Worker:IWorker
    {
        
        protected readonly IBusClient<AdvancedMessageContext> Client;
        protected Worker(IBusClient<AdvancedMessageContext>  client)
        {
            Client = client;

        }

        public WorkerInstanceConfiguration Configuration { get; set; }
        public abstract void Initialize();
    }
}