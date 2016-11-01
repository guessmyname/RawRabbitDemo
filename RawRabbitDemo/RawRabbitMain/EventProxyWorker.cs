using System;
using System.Threading.Tasks;

namespace RawRabbitMain
{
    public class EventProxyWorker:ProxyWorker
    {

        public EventProxyWorker()
        {
            SubscribeAsync<EventMsg>(async (msg, context) =>
            {
                var t1 = GetMessageFromService();
                var t2 = GetMessageFromMongo(msg.Key, "Configuration");
                var t3 = GetMessageFromMongo(msg.Key, "Transaction");

                await Task.WhenAll(t1, t2,t3);

                
                if (!string.IsNullOrEmpty(t3.Result))
                {
                    var newMessage = MergeResults(t1.Result, t2.Result, t3.Result);
                    await PublishAsync(newMessage);
                }
               

            });
        }

        private object MergeResults(string result, string s, string result1)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetMessageFromMongo(string key, string collection)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetMessageFromService()
        {

            await Task.Yield();

            return null;
        }
    }
}