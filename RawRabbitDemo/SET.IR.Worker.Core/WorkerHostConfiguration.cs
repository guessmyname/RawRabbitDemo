using System.Collections.Generic;

namespace SET.IR.Worker.Core
{
    public class WorkerHostConfiguration
    {
        public string SystemName { get; set; }

        public List<WorkerConfiguration> WorkerConfigurations { get; set; }
    }
}