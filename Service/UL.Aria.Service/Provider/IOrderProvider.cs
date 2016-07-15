using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IOrderProvider
    /// </summary>
    public interface IOrderProvider
    {
		/// <summary>
		/// Creates the specified new order.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="orderXml">The new order.</param>
		/// <returns>Guid.</returns>
        Guid Create(string messageId, string orderXml);

		/// <summary>
		/// Updates the specified order id.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="orderXml">The order.</param>
		void Update(string messageId, string orderXml);


        /// <summary>
        ///     Deletes the specified <see cref="Order" />.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);

        /// <summary>
        ///     Finds the <see cref="Order" /> by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        Order FindById(Guid entityId);

        /// <summary>
        ///     Finds the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        Order FindByOrderNumber(string orderNumber);

        /// <summary>
        /// Fetches the order lookups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lookup> FindOrderLookups();
    }
}