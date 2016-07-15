using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IContainerProvider
    /// </summary>
    public interface IContainerProvider
    {
        /// <summary>
        ///     Creates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Guid.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        Guid Create(Container container);

        /// <summary>
        ///     Updates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        void Update(Container container);

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>Container.</returns>
        Container GetById(Guid containerId);

        /// <summary>
        ///     Gets the by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns>IEnumerable{Container}.</returns>
        IEnumerable<Container> GetByCompanyId(Guid companyId);

        /// <summary>
        ///     Deletes the specified container id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        void Delete(Guid containerId);

        /// <summary>
        /// Deletes the list.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="name">The name.</param>
        void DeleteList(Guid containerId, string name);

		/// <summary>
		/// Fetches the by primary search entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <returns>Container.</returns>
	    Container FetchByPrimarySearchEntityId(Guid primarySearchEntityId);
    }
}