using Microsoft.Practices.Unity;

using UL.Aria.Service.InboundOrderProcessing.MessageProcessor;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Class MessageProcessorResolver.
    /// </summary>
    public sealed class MessageProcessorResolver : IMessageProcessorResolver
    {
        private readonly IUnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageProcessorResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MessageProcessorResolver(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IMessageProcessor.</returns>
        public IMessageProcessor Resolve(string name)
        {
            return _container.Resolve<IMessageProcessor>(name);
        }
    }
}