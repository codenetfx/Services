using UL.Aria.Service.InboundOrderProcessing.Domain;
using UL.Aria.Service.InboundOrderProcessing.Repository;

namespace UL.Aria.Service.InboundOrderProcessing.Provider
{
    /// <summary>
    /// Implements a message provider.
    /// </summary>
    public class InboundOrderProvider : IInboundOrderProvider
    {
        /// <summary>
        /// The _order message repository
        /// </summary>
        private readonly IInboundOrderRepository _inboundOrderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundOrderProvider" /> class.
        /// </summary>
        /// <param name="inboundOrderRepository">The inbound order repository.</param>
        /// <returns></returns>
        public InboundOrderProvider(IInboundOrderRepository inboundOrderRepository)
        {
            _inboundOrderRepository = inboundOrderRepository;
        }

        /// <summary>
        /// Enqueues the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Enqueue(OrderMessage message)
        {
            _inboundOrderRepository.Create(message);
        }

        /// <summary>
        /// Dequeues the top message.
        /// </summary>
        /// <returns></returns>
        public OrderMessage Dequeue()
        {
            return _inboundOrderRepository.FetchNextForProcessing();
        }
    }
}