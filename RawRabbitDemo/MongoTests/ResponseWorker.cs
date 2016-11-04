using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.Context.Enhancer;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;
using RawRabbitMain;

namespace MongoTests
{
   public abstract class ResponseWorker<TRequest,TResponse>
    {
        private readonly WorkerConfiguration _configuration;
        private readonly IBusClient<DetailedContext> _client;
        protected ResponseWorker(WorkerConfiguration configuration)
        {
            _configuration = configuration;

            var cfg = RawRabbitConfiguration.Local.AsLegacy();
            _client = BusClientFactory.CreateDefault<DetailedContext>(null, config =>
            {

                config.AddSingleton<IContextEnhancer, DetailedContextEnhancer>();
                config.AddSingleton(s => cfg);

            });

            Initialize();
        }

       private void Initialize()
       {
            _client.RespondAsync<TRequest, TResponse>(async (request, context) => await HandleRequest(request, context));
        }


       protected abstract Task<TResponse> HandleRequest<TContext>(TRequest request, TContext context);
    }
}
