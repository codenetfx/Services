using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IIncomingOrderRepository
    /// </summary>
    public interface IIncomingOrderRepository
    {
        /// <summary>
        ///     Creates the specified <see cref="IncomingOrder" />
        /// </summary>
        /// <param name="incomingOrder">The new order mock.</param>
        /// <returns>Guid.</returns>
        Guid Create(IncomingOrder incomingOrder);

        /// <summary>
        ///     Updates the specified <see cref="IncomingOrder" />.
        /// </summary>
        /// <param name="incomingOrder">The incoming order.</param>
        /// <returns>System.Int32.</returns>
        int Update(IncomingOrder incomingOrder);

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="contact">The contact.</param>
        void UpdateAllContactsForExternalId(string externalId, IncomingOrderContact contact);

        /// <summary>
        /// Updates all customers for external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="contact">The contact.</param>
        void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer contact);

        /// <summary>
        ///     Deletes the specified <see cref="IncomingOrder" />.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);

        /// <summary>
        ///     Deletes the service line <see cref="IncomingOrderServiceLine" />.
        /// </summary>
        /// <param name="id">The service line id.</param>
        void DeleteServiceLine(Guid id);

        /// <summary>
        ///     Searches for <see cref="IncomingOrder" /> objects using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        IncomingOrderSearchResultSet Search(SearchCriteria searchCriteria);

        /// <summary>
        ///     Finds the incoming order by id.
        /// </summary>
        /// <param name="entityId">The incoming order id.</param>
        /// <returns></returns>
        IncomingOrder FindById(Guid entityId);

        /// <summary>
        ///     Finds the incoming order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IncomingOrder.</returns>
        IncomingOrder FindByOrderNumber(string orderNumber);

        /// <summary>
        ///     Finds the by service line id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        IncomingOrder FindByServiceLineId(Guid entityId);

        /// <summary>
        ///     Adds the service line.
        /// </summary>
        /// <param name="serviceLine">The service line.</param>
        void AddServiceLine(IncomingOrderServiceLine serviceLine);
        
        /// <summary>
        /// Fetches the Incoming Order lookups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lookup> FetchIncomingOrderLookups();

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        void UpdateContact(IncomingOrderContact contact);

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
	    Guid CreateContact(IncomingOrderContact contact);
    }
}