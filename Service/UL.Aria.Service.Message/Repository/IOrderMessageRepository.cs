using System;
using UL.Aria.Service.Message.Domain;

namespace UL.Aria.Service.Message.Repository
{
    /// <summary>
    /// Defines operations for order message repository.
    /// </summary>
    public interface IOrderMessageRepository
    {
        /// <summary>
        /// Creates the specified x
        /// </summary>
        /// <param name="orderMessage">The order message.</param>
        void Create(OrderMessage orderMessage);

        /// <summary>
        /// Fetches an <see cref="OrderMessage"/> by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        OrderMessage FetchById(Guid id);

        /// <summary>
        /// Fetches the next <see cref="OrderMessage"/> for processing.
        /// </summary>
        /// <returns></returns>
        OrderMessage FetchNextForProcessing();
    }
}
