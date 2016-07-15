using System;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Interface defining a Order Manager class.
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Gets the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Order.</returns>
        Order FetchById(Guid id);

        /// <summary>
        /// Gets the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        Order FetchByOrderNumber(string orderNumber);

		/// <summary>
		/// Creates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		/// <returns>The created order id.</returns>
        Guid Create(string orderXml);

		/// <summary>
		/// Updates the order.
		/// </summary>
		/// <param name="orderXml">The order.</param>
		void Update(string orderXml);

        /// <summary>
        /// Deletes the order by id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);
    }
}
