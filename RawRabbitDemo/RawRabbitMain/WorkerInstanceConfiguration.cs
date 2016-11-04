using System.Collections.Generic;
using System.Linq;
using RawRabbit.Common;
using RawRabbit.Configuration.Subscribe;
using RawRabbit.Context;
using RawRabbit.ErrorHandling;
using RawRabbit.Operations;

namespace RawRabbitMain
{
    public class WorkerInstanceConfiguration
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public List<string> RoutingKeys { get; set; }

        public int AllowedRetries { get; set; }

        public dynamic CustomSettings { get; set; }
    }
}
