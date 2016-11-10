using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
    public interface IWorker
    {

          WorkerInstanceConfiguration Configuration { get; set; }
        IBusClient<AdvancedMessageContext> Client { get; set; }
        void Init();
    }
}