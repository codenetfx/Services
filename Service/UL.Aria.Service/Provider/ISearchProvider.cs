using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     A search provider
    /// </summary>
    public interface ISearchProvider
    {
        /// <summary>
        ///     Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        SearchResultSet Search(SearchCriteria searchCriteria);

        /// <summary>
        ///     Creates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerMetadata">The container metadata.</param>
        /// <returns>The created entity id</returns>
        Guid Create(Container container, string containerMetadata);

        /// <summary>
        ///     Updates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="containerMetadata">The container metadata.</param>
        void Update(Container container, string containerMetadata);

		/// <summary>
		/// Gets the products for the specified project.
		/// </summary>
		/// <param name="projectId">The project unique identifier.</param>
		/// <returns></returns>
	    IList<Guid> FetchProductsByProjectId(Guid projectId);

		
    }
}