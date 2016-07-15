using UL.Aria.Service.InboundOrderProcessing.Domain;

namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
    /// <summary>
    ///     Interface IMessageProcessor
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        ///     Processes the message.
        /// </summary>
        /// <param name="orderMessage">The order message.</param>
        void ProcessMessage(OrderMessage orderMessage);
    }
}