using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using RawRabbitMain;

namespace MongoTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
           var result = client.GetStringAsync("posts/1").Result;


            var mongoMsg = new MongoEnvelope("12042016", "12042016A", "Model", result);

            var mongo = new MongoClient("mongodb://alvsetwdev002ad.corpdev1.jmfamily.com:27017");

            var db = mongo.GetDatabase("IRv3");

            var coll = db.GetCollection<MongoEnvelope>("Vehicles");

            coll.InsertOne(mongoMsg);
        }

        [TestMethod]

        public void GetSubDocument()
        {
            var mongo = new MongoClient("mongodb://alvsetwdev002ad.corpdev1.jmfamily.com:27017");

            var db = mongo.GetDatabase("IRv3");

            var coll = db.GetCollection<MongoEnvelope>("Vehicles");

            var doc = coll.FindAsync(msg => msg.EnvelopeId == "12042016").Result.First();
        }
        
     
        
    }
}
