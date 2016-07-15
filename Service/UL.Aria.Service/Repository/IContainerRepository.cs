using System;
using System.Collections.Generic;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     The content repository
    /// </summary>
    public interface IContainerRepository : IRepositoryBase<Container>
    {
        /// <summary>
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Guid.</returns>
        Guid Create(Container entity);

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
        /// <exception cref="System.NotImplementedException"></exception>
        IEnumerable<Container> GetByCompanyId(Guid companyId);

        /// <summary>
        /// Deletes the list.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="name">The name.</param>
        void DeleteList(Guid entityId, string name);

		/// <summary>
		/// Gets the by primary search entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <returns>Container.</returns>
	    Container GetByPrimarySearchEntityId(Guid primarySearchEntityId);
    }
}