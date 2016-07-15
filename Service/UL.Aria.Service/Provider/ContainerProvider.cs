using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Container provider implementation for entities like products, orders and projects.
    /// </summary>
    public sealed class ContainerProvider : IContainerProvider
    {
        private readonly IContainerRepository _containerRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerProvider" /> class.
        /// </summary>
        /// <param name="containerRepository">The container repository.</param>
        public ContainerProvider(IContainerRepository containerRepository)
        {
            _containerRepository = containerRepository;
        }

        /// <summary>
        ///     Creates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Guid.</returns>
        public Guid Create(Container container)
        {
            return _containerRepository.Create(container);
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>Container.</returns>
        public Container GetById(Guid containerId)
        {
            return _containerRepository.GetById(containerId);
        }

        /// <summary>
        ///     Gets the by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns>IEnumerable{Container}.</returns>
        public IEnumerable<Container> GetByCompanyId(Guid companyId)
        {
            return _containerRepository.GetByCompanyId(companyId);
        }

        /// <summary>
        ///     Updates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Update(Container container)
        {
            _containerRepository.Update(container);
        }

        /// <summary>
        ///     Deletes the specified container id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        public void Delete(Guid containerId)
        {
            _containerRepository.Remove(containerId);
        }

        /// <summary>
        /// Deletes the list.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="name">The name.</param>
        public void DeleteList(Guid containerId, string name)
        {
            _containerRepository.DeleteList(containerId, name);
        }

		/// <summary>
		/// Fetches the by entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <returns>Container.</returns>
	    public Container FetchByPrimarySearchEntityId(Guid primarySearchEntityId)
	    {
			return _containerRepository.GetByPrimarySearchEntityId(primarySearchEntityId);
		}
    }
}