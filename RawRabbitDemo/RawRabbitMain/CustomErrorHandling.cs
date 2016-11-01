using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RawRabbit.Channel.Abstraction;
using RawRabbit.Common;
using RawRabbit.Configuration.Subscribe;
using RawRabbit.Consumer.Abstraction;
using RawRabbit.ErrorHandling;
using RawRabbit.Serialization;

namespace RawRabbitMain
{
  public class CustomErrorHandling:DefaultStrategy
    {
      public CustomErrorHandling(IMessageSerializer serializer, INamingConventions conventions, IBasicPropertiesProvider propertiesProvider, ITopologyProvider topologyProvider, IChannelFactory channelFactory) : base(serializer, conventions, propertiesProvider, topologyProvider, channelFactory)
      {
      }

      public override Task ExecuteAsync(Func<Task> messageHandler, Func<Exception, Task> errorHandler)
      {
          return base.ExecuteAsync(messageHandler, errorHandler);
      }

      public override Task OnSubscriberExceptionAsync(IRawConsumer consumer, SubscriptionConfiguration config, BasicDeliverEventArgs args,
          Exception exception)
      {
          throw exception;
      }
    }
}
