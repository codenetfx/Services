using UL.Aria.Service.InboundOrderProcessing.MessageProcessor;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Interface IMessageProcessorResolver
    /// </summary>
    public interface IMessageProcessorResolver
    {
        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IMessageProcessor.</returns>
        IMessageProcessor Resolve(string name);
    }
}