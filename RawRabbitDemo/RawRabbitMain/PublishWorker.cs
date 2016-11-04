using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawRabbit.Configuration.Publish;
using RawRabbit.Exceptions;

namespace RawRabbitMain
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

    public abstract class PublishWorker:Worker, IPublishWorker
    {
       protected PublishWorker(WorkerInstanceConfiguration configuration) : base(configuration)
       {
       }


        /// <summary>
        /// Method will publish to RabbitMQ. If message is delivered within 
        /// PublishConfirmTimeout true will be returned, otherwise false will be returned.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="messageId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<bool> Publish(object msg, Guid messageId = default(Guid), Action<IPublishConfigurationBuilder> config = null)
        {
            try
            {
                await Client.PublishAsync(msg, messageId, config);

            }
            catch (PublishConfirmException)
            {
                return false;
            }

            return true;


        }

    }
}
