using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IOrderRepository
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        ///     Creates the specified <see cref="Order" />
        /// </summary>
        /// <param name="order">The new order.</param>
        /// <returns>Guid.</returns>
        Guid Create(Order order);

        /// <summary>
        ///     Updates the specified <see cref="Order" />.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>System.Int32.</returns>
        int Update(Order order);

        /// <summary>
        ///     Deletes the specified <see cref="Order" />.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);

        /// <summary>
        ///     Finds the order by id.
        /// </summary>
        /// <param name="entityId">The order id.</param>
        /// <returns></returns>
        Order FindById(Guid entityId);

        /// <summary>
        ///     Finds the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        Order FindByOrderNumber(string orderNumber);

        /// <summary>
        /// Fetches the order lookups.
        /// </summary>
        /// <returns>A list of order lookups</returns>
        IEnumerable<Lookup> FetchOrderLookups();

        /// <summary>
        /// Finds the order lookups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lookup> FindOrderLookups();
    }
}