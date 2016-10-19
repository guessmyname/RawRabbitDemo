using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Attributes;
using RawRabbit.Configuration.Exchange;

namespace RawRabbitMain
{
    
    [Routing( NoAck = true, PrefetchCount = 50)]
    public class HeartbeatMessage
    {
        public string MessagesProcessed { get; set; }

        public DateTime RecordedTime { get; set; }
    }
}
