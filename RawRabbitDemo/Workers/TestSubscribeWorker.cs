using System;
using System.Threading.Tasks;
using RawRabbit.Context;
using SET.IR.Worker.Core;
using RawRabbit.vNext.Disposable;

namespace WorkerTest
{
   public class TestSubscribeWorker:SubscribeWorker<TestMessage>
    {
      

       protected override async Task HandleMessage(TestMessage message, long retryCount, Action<TimeSpan> retryLater)
       {
          throw new Exception();
       }


    }
}
