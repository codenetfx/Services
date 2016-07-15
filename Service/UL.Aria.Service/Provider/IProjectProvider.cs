using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IProjectProvider
    /// </summary>
    public interface IProjectProvider
    {
        /// <summary>
        ///     Fetches the projects.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>IEnumerable{Project}.</returns>
        IEnumerable<Project> FetchProjects(IEnumerable<Guid> ids);

        /// <summary>
        ///     Publishes the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="changedProject">The changed project.</param>
        void Publish(Project project, Project changedProject);

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Project.</returns>
        Project FetchById(Guid id);

        /// <summary>
        ///     Fetches the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IList{Project}.</returns>
        IList<Project> FetchByOrderNumber(string orderNumber);

        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="project">The project.</param>
        /// <returns>Guid.</returns>
        Guid Create(Guid containerId, Project project);

        /// <summary>
        ///     Updates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        void Update(Project project);

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
        ///     Updates from incoming order.
        /// </summary>
        /// <param name="project">The project.</param>
        void UpdateFromIncomingOrder(Project project);

        /// <summary>
        /// Updates the status from order.
        /// </summary>
        /// <param name="project">The project.</param>
        void UpdateStatusFromOrder(Project project);

        /// <summary>
        ///     Fetches all projects.
        /// </summary>
        /// <returns>IEnumerable{Project}.</returns>
        IEnumerable<Project> FetchProjects();

        /// <summary>
        /// Fetches the project lookups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lookup> FetchProjectLookups();


        /// <summary>
        /// Fetches all headers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Guid> FetchAllIds();

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="contact">The contact.</param>
        void UpdateContact(Guid projectId, IncomingOrderContact contact);

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
	    Guid CreateContact(Guid projectId, IncomingOrderContact contact);
    }
}