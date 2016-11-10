using System;
using System.Collections.Generic;
using RawRabbit.Context;
using RawRabbit.vNext.Disposable;

namespace SET.IR.Worker.Core
{
   public class WorkerHost
   {

       private List<IWorker> workers = new List<IWorker>();

       public WorkerHost(IServiceProvider provider, WorkerHostConfiguration config)
       {
            
           foreach (var workerConfiguration in config.WorkerConfigurations)
           {
               for (int i = 0; i < workerConfiguration.NumberOfInstances; i++)
               {
                   var type = Type.GetType(workerConfiguration.Type);

                   if (type != null && typeof(IWorker).IsAssignableFrom(type))
                   {
                       var instance = provider.GetService(type) as IWorker;

                       if (instance != null)
                       {
                           var client = provider.GetService(typeof(IBusClient<AdvancedMessageContext>)) as IBusClient<AdvancedMessageContext>;
                           instance.Configuration = workerConfiguration.InstanceConfiguration;
                           instance.Client = client;
                           instance.Init();
                           workers.Add(instance);
                       }
                   }
                   else
                   {
                       //TODO Log failure to find type.
                   }
                  

               }
              
            }
       }
    }
}
