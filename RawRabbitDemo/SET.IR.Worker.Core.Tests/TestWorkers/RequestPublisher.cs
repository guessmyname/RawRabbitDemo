using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET.IR.Worker.Core.Tests.TestWorkers
{
   public class RequestPublisher:RequestReplyWorker<string,string>
    {
        protected override async Task<string> HandleRequest<TContext>(string request, TContext context)
        {
            await Task.Yield();

            return request;
        }
    }
}
