namespace SET.IR.Worker.Core
{
    public interface IWorker
    {

          WorkerInstanceConfiguration Configuration { get; set; }
        void Initialize();
    }
}