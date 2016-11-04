using System.Collections.Generic;
using RawRabbit.Configuration.Exchange;

namespace SET.IR.Worker.Core
{
    public class WorkerInstanceConfiguration
    {
        public string ExchangeName { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public List<string> RoutingKeys { get; set; }

        public int AllowedRetries { get; set; }

        public List<PublishConfig> PublishToConfig { get; set; }

        public dynamic CustomSettings { get; set; }
    }

    public class PublishConfig
    {
        public string Name { get; set; }
        public string ExchangeName { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public string RoutingKey { get; set; }
    }

}
