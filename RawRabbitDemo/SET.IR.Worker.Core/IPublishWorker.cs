using System;
using System.Threading.Tasks;
using RawRabbit.Configuration.Publish;

namespace SET.IR.Worker.Core
{
    public interface IPublishWorker
    {
        /// <summary>
        /// Method will publish to RabbitMQ. If message is delivered within 
        /// PublishConfirmTimeout true will be returned, otherwise false will be returned.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="messageId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        Task<bool> Publish(object msg, Guid messageId = default(Guid), Action<IPublishConfigurationBuilder> config = null);
    }
}