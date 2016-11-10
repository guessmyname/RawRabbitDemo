using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SET.IR.Worker.Core
{
   public class WorkerHostLoader:IDisposable
    {
       private readonly IServiceProvider _service;
       private readonly string _mongoConnectionString;
       private readonly string _database;
       private readonly string _collection;

       public WorkerHostLoader(IServiceProvider service, string mongoConnectionString, string database, string collection)
       {
           _service = service;
           _mongoConnectionString = mongoConnectionString;
           _database = database;
           _collection = collection;
       }

       readonly ManualResetEvent _finished = new ManualResetEvent(false);
       public void Load(List<string> systemNames)
       {

            MongoClient client = new MongoClient(_mongoConnectionString);
            var db = client.GetDatabase(_database);
           var col = db.GetCollection<WorkerhostConfig>(_collection).AsQueryable();

           var config = col.Where(e => systemNames.Contains(e.SystemName)).ToList();

           foreach (var workerhostConfig in config)
           {
                var host = new WorkerHost(_service, workerhostConfig);
            }

           _finished.WaitOne();
       }

       public class WorkerhostConfig: WorkerHostConfiguration
        {

           public ObjectId Id { get; set; }
       }

        
       public void Dispose()
       {
           _finished.Set();
       }
    }
}
