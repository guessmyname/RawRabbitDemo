using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionActivator;

namespace RawRabbitMain
{
   public class WorkerHost
   {

       private List<IWorker> workers = new List<IWorker>();

       public WorkerHost(WorkerHostConfiguration config)
       {
           foreach (var workerConfiguration in config.WorkerConfigurations)
           {
               for (int i = 0; i < workerConfiguration.NumberOfInstances; i++)
               {
                   var type = Type.GetType(workerConfiguration.Type);

                   if (type != null && type.IsAssignableFrom(typeof(IWorker)))
                   {
                       var activator = type.CreateActivator(typeof(WorkerInstanceConfiguration));

                       var instance = activator.Invoke(workerConfiguration.InstanceConfiguration) as IWorker;

                        workers.Add(instance);
                   }
                   else
                   {
                       //TODO Log failure to find type.
                   }
                  

               }
              
            }
       }
    }

    public class WorkerHostConfiguration
    {
        public string SystemName { get; set; }

        public List<WorkerConfiguration> WorkerConfigurations { get; set; }
    }

    public class WorkerConfiguration
    {
        public string Type { get; set; }
        public int NumberOfInstances { get; set; }

        public WorkerInstanceConfiguration InstanceConfiguration { get; set; }
    }
}
