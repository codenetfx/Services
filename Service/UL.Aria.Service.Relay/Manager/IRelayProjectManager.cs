using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Relay.Domain;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Defines operations for fetching project for internal UL services.
    /// </summary>
    public interface IRelayProjectManager
    {
        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        ProjectDto GetProjectById(Guid id);

        /// <summary>
        /// Searches the projects.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        RelaySearchResultSet<ProjectDto> SearchProjects(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Fetches all documents.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        SearchResultSetDto GetAllProjectDocuments(Guid projectId);

        /// <summary>
        /// Fetches the project handler.
        /// </summary>
        /// <param name="projectHandler">The project handler.</param>
        /// <returns></returns>
        ProfileDto FetchProfile(string projectHandler);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	    bool Ping();

        /// <summary>
        /// Searches the project task summary.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        RelaySearchResultSet<TaskDto> SearchProjectTaskSummary(SearchCriteriaDto searchCriteria);
    }
}
