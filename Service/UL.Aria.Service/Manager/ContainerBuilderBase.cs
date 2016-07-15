using System;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Class to build a <see cref="Container"/> for a <see cref="PrimarySearchEntityBase"/>
    /// </summary>
    public abstract class ContainerBuilderBase : IContainerBuilder
    {
        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="containerId"></param>
        /// <returns>Container.</returns>
        public Container Create(PrimarySearchEntityBase primarySearchEntityBase, Guid containerId)
        {

            var container = new Container
            {
                Id = containerId,
                CompanyId = primarySearchEntityBase.CompanyId,
                PrimarySearchEntityId =
                    primarySearchEntityBase.Id.HasValue ? primarySearchEntityBase.Id.Value : Guid.Empty,
                PrimarySearchEntityType = primarySearchEntityBase.Type.ToString()
            };
            BuildContainer(primarySearchEntityBase, container);
            return container;
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="container">The container.</param>
        protected abstract void BuildContainer(PrimarySearchEntityBase primarySearchEntityBase, Container container);
    }
}