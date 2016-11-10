#define CONTRACTS_FULL
using System;
using System.Diagnostics.Contracts;
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
        public IBusClient<AdvancedMessageContext> Client { get; set; }

        public WorkerInstanceConfiguration Configuration { get; set; }
        protected abstract void Initialize();

        public void Init()
        {

            Contract.Requires(Client != null, "Client must be assigned before calling Init");

            Contract.Requires(Configuration != null, "Configuration must be assigned before calling Init");
            Initialize();
        }

    }
}