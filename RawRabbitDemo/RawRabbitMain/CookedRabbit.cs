using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawRabbitMain
{
   public class CookedRabbit:RabbitWatch
    {
        public CookedRabbit(string systemName, string rabbitName, bool enableHeartbeat = true) : base(systemName, rabbitName, enableHeartbeat)
        {
        }
    }
}
