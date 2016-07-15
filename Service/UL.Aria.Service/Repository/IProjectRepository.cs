using System;
using System.Collections;
using System.Collections.Generic;
using UL.Aria.Service.Auditing;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface IProjectRepository
    /// </summary>
    [Audit]
    public interface IProjectRepository
    {
        /// <summary>
        /// Fetches the project lookups.
        /// </summary>
        /// <returns></returns>
        [AuditIgnore]
        IEnumerable<Lookup> FetchProjectLookups();
        
        /// <summary>
        ///     Fetches the projects.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>IList{Project}.</returns>
        [AuditIgnore]
        IEnumerable<Project> FetchProjects(IEnumerable<Guid> ids);

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Project.</returns>
        [AuditIgnore]
        Project FetchById(Guid id);

        /// <summary>
        ///     Fetches the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IList{Project}.</returns>
        [AuditIgnore]
        IList<Project> FetchByOrderNumber(string orderNumber);

        /// <summary>
        ///     Creates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Guid.</returns>
        [AuditResource("project")]
        Guid Create(Project project);

        /// <summary>
        ///     Updates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        [AuditResource("project")]
        void Update(Project project);

        /// <summary>
        ///     Updates Project Status and LineItems Statuses only.
        /// </summary>
        /// <param name="project">The project.</param>
        [AuditResource("project")]
        void UpdateStatusFromOrder(Project project);

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="contact">The contact.</param>
        [AuditIgnore]
        void UpdateAllContactsForExternalId(string externalId, IncomingOrderContact contact);

        /// <summary>
        /// Updates all customers for external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="customer">The contact.</param>
        [AuditIgnore]
        void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer customer);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [AuditIgnore]
        void Delete(Guid id);

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{Project}.</returns>
        [AuditIgnore]
        IList<Project> FetchAll();

        /// <summary>
        ///     Updates from incoming order.
        /// </summary>
		/// <param name="project">The entity.</param>
        /// <exception cref="DatabaseItemNotFoundException"></exception>
        [AuditResource("project")]
		void UpdateFromIncomingOrder(Project project);

        /// <summary>
        /// Fetches all headers.
        /// </summary>
        /// <returns></returns>
        [AuditIgnore]
        IEnumerable<Guid> FetchAllIds();

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="contact">The contact.</param>
        [AuditIgnore]
        void UpdateContact(Guid projectId, IncomingOrderContact contact);

	    /// <summary>
	    /// Creates the contact.
	    /// </summary>
	    /// <param name="projectId">The project identifier.</param>
	    /// <param name="contact">The contact.</param>
	    /// <returns>Guid.</returns>
	    [AuditIgnore]
	    Guid CreateContact(Guid projectId, IncomingOrderContact contact);
    }
}