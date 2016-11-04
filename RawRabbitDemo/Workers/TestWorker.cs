using System;
using System.Threading.Tasks;
using RawRabbitMain;

namespace WorkerTest
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
