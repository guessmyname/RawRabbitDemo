using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Workers
{
    public class EntityEnvelope
    {
        public string EnvelopeId { get; set; }
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime DeliverAfter { get; set; }
        public dynamic Data { get;  set; }
        public byte[] Hash { get; set; }
    }

    public class MongoEnvelope : EntityEnvelope
    {
      
        public ObjectId Id { get; set; }

        public MongoEnvelope(string envelopeId, string itemId, string itemType, string data, DateTime? deliverAfterTime = null) 
        {
            EnvelopeId = envelopeId;
            ItemId = itemId;
            ItemType = itemType;
            Data = JsonConvert.DeserializeObject<dynamic>(data);
            CreatedTime = DateTime.Now;
            DeliverAfter = deliverAfterTime ?? DateTime.Now;
            Hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public MongoEnvelope()
        {
            
        }
    }
}
