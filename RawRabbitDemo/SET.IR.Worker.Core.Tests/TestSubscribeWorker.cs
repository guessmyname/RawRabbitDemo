using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core.Tests
{
   public class TestSubscribeWorker:SubscribeWorker<TestMessage>
    {
       private readonly Action<TestMessage> _testMethod;

       public TestSubscribeWorker(Action<TestMessage> testMethod) 
       {
           _testMethod = testMethod;
       }

       protected override async Task HandleMessage(TestMessage message, long retryCount, Action<TimeSpan> retryLater)
       {
           _testMethod(message);
            
       }
    }
}
