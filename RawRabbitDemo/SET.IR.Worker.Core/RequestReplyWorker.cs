using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Context;
using RawRabbit.Context.Enhancer;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
   public abstract class RequestReplyWorker<TRequest,TResponse>:Worker
    {
      

       protected override void Initialize()
       {
            Client.RespondAsync<TRequest, TResponse>(async (request, context) => await HandleRequest(request, context));
        }


       protected abstract Task<TResponse> HandleRequest<TContext>(TRequest request, TContext context);
    }
}
