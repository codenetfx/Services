using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Interface IContainerManager
    /// </summary>
    public interface IContainerManager
    {
        /// <summary>
        ///     Deletes the specified container by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        void Delete(Guid containerId);

        /// <summary>
        ///     Fetches the specified container by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>Container.</returns>
        Container FindById(Guid containerId);

        /// <summary>
        ///     Updates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        void Update(Container container);

        /// <summary>
        ///     Creates entity metadata.
        /// </summary>
        /// <param name="primarySearchEntityBase">The container.</param>
        /// <param name="containerId"></param>
        /// <returns>The created content id.</returns>
        Guid Create(PrimarySearchEntityBase primarySearchEntityBase, Guid? containerId = null);

        /// <summary>
        ///     Creates entity metadata.
        /// </summary>
        /// <param name="primarySearchEntityBase">The container.</param>
        /// <returns>
        ///     The created content id.
        /// </returns>
        Guid Create(PrimarySearchEntityBase primarySearchEntityBase);

        /// <summary>
        ///     Updates the specified primary search entity base.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="containerId">The container id.</param>
        void Update(PrimarySearchEntityBase primarySearchEntityBase, Guid? containerId = null);

        /// <summary>
        ///     Gets all actions.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        IList<System.Security.Claims.Claim> GetAvailableUserClaims(Guid containerId);


        /// <summary>
        ///     Get all containers by company id
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        SearchResultSet GetByCompanyId(SearchCriteria searchCriteria);
    }
}