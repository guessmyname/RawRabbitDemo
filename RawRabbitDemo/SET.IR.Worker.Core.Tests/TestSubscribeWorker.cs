using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core.Tests
{
   public class TestSubscribeWorker: SubscribeWorkerBase<TestMessage>
    {
       private readonly Func<TestMessage, Task> _testMethod;

       public TestSubscribeWorker(Func<TestMessage,Task> testMethod) 
       {
           _testMethod = testMethod;
       }
        

        protected override void AddMessageHandlers(RegisterMessageHandler registerMessageHandler)
        {
            registerMessageHandler( async message => await _testMethod(message));
        }
    }
}
