using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Class ContainerDefinitionBuilder
    /// </summary>
    public class ContainerDefinitionBuilder : IContainerDefinitionBuilder
    {
        private readonly Dictionary<EntityTypeEnumDto, IContainerBuilder> _containerBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerDefinitionBuilder"/> class.
        /// </summary>
        /// <param name="containerBuilders">The container builders.</param>
        public ContainerDefinitionBuilder(Dictionary<EntityTypeEnumDto, IContainerBuilder> containerBuilders)
        {
            _containerBuilders = containerBuilders;
        }

        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="containerId"></param>
        /// <returns>Container.</returns>
        public Container Create(PrimarySearchEntityBase primarySearchEntityBase, Guid containerId)
        {
            return _containerBuilders[primarySearchEntityBase.Type].Create(primarySearchEntityBase, containerId);
        }
    }
}