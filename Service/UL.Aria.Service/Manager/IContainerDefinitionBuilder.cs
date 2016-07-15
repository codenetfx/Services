using System;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Interface IContainerDefinitionManager
    /// </summary>
    public interface IContainerDefinitionBuilder
    {
        /// <summary>
        /// Creates the specified container id.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="containerId"></param>
        /// <returns>Container.</returns>
        Container Create(PrimarySearchEntityBase primarySearchEntityBase, Guid containerId);
    }
}