using UL.Aria.Service.Message.Domain;
using UL.Aria.Service.Message.Repository;

namespace UL.Aria.Service.Message.Provider
{
    /// <summary>
    ///     Implements a message provider.
    /// </summary>
    public class OrderMessageProvider : IOrderMessageProvider
    {
        private readonly IOrderMessageRepository _orderMessageRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderMessageProvider" /> class.
        /// </summary>
        /// <param name="orderMessageRepository">The order message repository.</param>
        public OrderMessageProvider(IOrderMessageRepository orderMessageRepository)
        {
            _orderMessageRepository = orderMessageRepository;
        }

        /// <summary>
        ///     Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Enqueue(OrderMessage message)
        {
            _orderMessageRepository.Create(message);
        }

        /// <summary>
        ///     Dequeues the top message.
        /// </summary>
        /// <returns></returns>
        public OrderMessage Dequeue()
        {
            return _orderMessageRepository.FetchNextForProcessing();
        }
    }
}