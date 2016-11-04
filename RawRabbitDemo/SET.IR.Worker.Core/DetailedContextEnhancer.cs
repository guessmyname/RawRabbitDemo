using RabbitMQ.Client.Events;
using RawRabbit.Channel.Abstraction;
using RawRabbit.Common;
using RawRabbit.Consumer.Abstraction;
using RawRabbit.Context;
using RawRabbit.Context.Enhancer;

namespace SET.IR.Worker.Core
{
   public class DetailedContextEnhancer:IContextEnhancer
    {
       private readonly IChannelFactory _channelFactory;
       private readonly INamingConventions _conventions;

       public DetailedContextEnhancer(IChannelFactory channelFactory, INamingConventions conventions)
       {
           _channelFactory = channelFactory;
           _conventions = conventions;
       }

       public void WireUpContextFeatures<TMessageContext>(TMessageContext context, IRawConsumer consumer, BasicDeliverEventArgs args) where TMessageContext : IMessageContext
       {
            var contextEnhancer = new ContextEnhancer(_channelFactory,_conventions);
            contextEnhancer.WireUpContextFeatures(context,consumer,args);

           var detailedContext = context as IDetailedContext;
           if (detailedContext == null)
           {
               return;
           }

           detailedContext.Exchange = args.Exchange;
           detailedContext.RoutingKey = args.Exchange;

       }
    }
}
