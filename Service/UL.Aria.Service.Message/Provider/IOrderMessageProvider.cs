using UL.Aria.Service.Message.Domain;

namespace UL.Aria.Service.Message.Provider
{
    /// <summary>
    /// Defines a provider for Order Messages.
    /// </summary>
    public interface IOrderMessageProvider
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