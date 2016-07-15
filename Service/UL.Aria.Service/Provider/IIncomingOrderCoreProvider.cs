using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IIncomingOrderCoreProvider
	/// </summary>
	public interface IIncomingOrderCoreProvider
	{
		/// <summary>
		///     Creates the specified new order.
		/// </summary>
		/// <param name="incomingOrder">The new order.</param>
		/// <returns>Guid.</returns>
		Guid Create(IncomingOrder incomingOrder);

		/// <summary>
		/// 
		///     Updates the specified incoming order id.
		/// </summary>
		/// <param name="incomingOrderId">The incoming order id.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		void Update(Guid incomingOrderId, IncomingOrder incomingOrder);

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
		/// <param name="customer"></param>
		void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer customer);

		/// <summary>
		/// Deletes the specified <see cref="IncomingOrder"/>.
		/// </summary>
		/// <param name="id">The id.</param>
		void Delete(Guid id);

		/// <summary>
		/// Searches for <see cref="IncomingOrder"/> objects using the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		IncomingOrderSearchResultSet Search(SearchCriteria searchCriteria);

		/// <summary>
		/// Finds the <see cref="IncomingOrder"/> by id.
		/// </summary>
		/// <param name="entityId">The entity id.</param>
		/// <returns></returns>
		IncomingOrder FindById(Guid entityId);

		/// <summary>
		/// Publishes the project creation request.
		/// </summary>
		/// <param name="projectCreationRequest">The project creation request.</param>
		/// <param name="additionalWorkCreateProjectTemplateTasks">The create project template tasks.</param>
		/// <returns>Guid.</returns>
		Guid PublishProjectCreationRequest(ProjectCreationRequest projectCreationRequest, Action<Guid, Guid, int> additionalWorkCreateProjectTemplateTasks);

		/// <summary>
		/// Finds the project by service line id.
		/// </summary>
		IncomingOrder FindByServiceLineId(Guid serviceLineId);

		/// <summary>
		///     Finds the by order number.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <returns>IncomingOrder.</returns>
		IncomingOrder FindByOrderNumber(string orderNumber);

		/// <summary>
		///     Adds the service line.
		/// </summary>
		/// <param name="serviceLine">The service line.</param>
		void AddServiceLine(IncomingOrderServiceLine serviceLine);

		/// <summary>
		/// Deletes the service line.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void DeleteServiceLine(Guid id);

		/// <summary>
		/// Fetches a IEnumerable of name id pair objects for Incoming orders as lookup objects.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Lookup> FetchIncomingOrderLookups();


		/// <summary>
		/// Updates the incoming order.
		/// </summary>
		/// <param name="incomingOrderId">The incoming order identifier.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		void UpdateIncomingOrder(Guid incomingOrderId, IncomingOrder incomingOrder);

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