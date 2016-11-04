namespace SET.IR.Worker.Core
{
    public class WorkerConfiguration
    {
        public string Type { get; set; }
        public int NumberOfInstances { get; set; }

        public WorkerInstanceConfiguration InstanceConfiguration { get; set; }
    }
}