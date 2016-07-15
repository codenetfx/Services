using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IOrderServiceLineDetailRepository
    /// </summary>
    public interface IOrderServiceLineDetailRepository
    {
        /// <summary>
        ///     Creates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        void Create(OrderServiceLineDetail orderServiceLineDetail);

        /// <summary>
        ///     Finds all order service line details.
        /// </summary>
        /// <returns>IList{OrderServiceLineDetail}.</returns>
        IList<OrderServiceLineDetail> FindAll();

        /// <summary>
        ///     Finds a order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns>OrderServiceLineDetail.</returns>
        OrderServiceLineDetail FindByIds(Guid orderId, string lineId);

        /// <summary>
        ///     Searches for order service line details by the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>OrderServiceLineDetailSearchResultSet.</returns>
        OrderServiceLineDetailSearchResultSet Search(SearchCriteria searchCriteria);

        /// <summary>
        ///     Updates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        /// <returns>System.Int32.</returns>
        int Update(OrderServiceLineDetail orderServiceLineDetail);

        /// <summary>
        ///     Deletes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        void Delete(Guid orderId, string lineId);
    }
}