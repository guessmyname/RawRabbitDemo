using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbitMain;

namespace MongoTests
{
   public class TestSubscribeWorker:SubscribeWorker<TestMessage>
    {
       public TestSubscribeWorker(WorkerInstanceConfiguration configuration) : base(configuration)
       {
       }

       protected override async Task HandleMessage(TestMessage message, long retryCount, Action<TimeSpan> retryLater)
       {
           throw new Exception();
       }
    }

    public class TestMessage : IMessage
    {
        public string Message { get; set; }
    }
}
