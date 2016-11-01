using System;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;

namespace RawRabbitMain
{
    public class MongoEnvelope
    {
        public MongoEnvelope(string envelopeId, string itemId, string itemType, string data, DateTime? deliverAfterTime = null)
        {
            EnvelopeId = envelopeId;
            ItemId = itemId;
            ItemType = itemType;
            Data = BsonDocument.Parse(data);
            CreatedTime = DateTime.Now;
            DeliverAfter = deliverAfterTime ?? DateTime.Now;
            Hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public ObjectId Id { get; set; }
      
        public string EnvelopeId { get; private set; }
        public string ItemId { get; private set; }

        public string ItemType { get; private set; }

        public DateTime CreatedTime { get; private set; }

        public DateTime DeliverAfter { get; set; }

        public BsonDocument Data { get; private set; }

        public byte[] Hash { get; set; }
    }
}