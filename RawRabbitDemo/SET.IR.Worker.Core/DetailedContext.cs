using RawRabbit.Context;

namespace SET.IR.Worker.Core
{
    public class DetailedContext : AdvancedMessageContext, IDetailedContext
    {

        public string Exchange { get; set; }

        public string RoutingKey { get; set; }
    }
}