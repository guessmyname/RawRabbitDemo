using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public interface IWorker
    {
        void Init(IBusClient<AdvancedMessageContext> Client, WorkerInstanceConfiguration Configuration);
    }
}