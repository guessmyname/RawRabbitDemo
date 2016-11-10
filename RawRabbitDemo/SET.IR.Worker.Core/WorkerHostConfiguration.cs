using System.Collections.Generic;
using RawRabbit.Configuration.Exchange;

namespace SET.IR.Worker.Core
{
    public class WorkerHostConfiguration
    {
        public string SystemName { get; set; }

        public List<WorkerConfiguration> WorkerConfigurations { get; set; }
    }

    public class WorkerConfiguration
    {
        public string Type { get; set; }
        public int NumberOfInstances { get; set; }

        public WorkerInstanceConfiguration InstanceConfiguration { get; set; }
    }


    public class WorkerInstanceConfiguration
    {
        
        public SubsciptionConfig SubsciptionConfig { get; set; }

        public ScheduledConfig ScheduledConfig { get; set; }

        public List<PublishConfig> PublishToConfig { get; set; }

        public dynamic CustomConfig { get; set; }
    }

    public class PublishConfig
    {
        public string Name { get; set; }
        public string ExchangeName { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public string RoutingKey { get; set; }
    }

    public class SubsciptionConfig
    {
        public string ExchangeName { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public List<string> RoutingKeys { get; set; }

        public int AllowedRetries { get; set; }
    }

    public class ScheduledConfig
    {
        public int IntervalSeconds { get; set; }
    }
}