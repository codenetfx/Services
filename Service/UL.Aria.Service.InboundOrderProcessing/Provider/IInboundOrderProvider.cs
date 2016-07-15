using UL.Aria.Service.InboundOrderProcessing.Domain;

namespace UL.Aria.Service.InboundOrderProcessing.Provider
{
    /// <summary>
    /// Defines a provider for Order Messages.
    /// </summary>
    public interface IInboundOrderProvider
    {
        /// <summary>
        ///     Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Enqueue(OrderMessage message);

        /// <summary>
        ///     Dequeues the top message.
        /// </summary>
        /// <returns></returns>
        OrderMessage Dequeue();
    }
}