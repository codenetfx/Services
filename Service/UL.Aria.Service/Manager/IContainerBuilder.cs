using System;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations to build a <see cref="Container"/> for a <see cref="PrimarySearchEntityBase"/>
    /// </summary>
    public interface IContainerBuilder
    {
        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="primarySearchEntityBase">The primary search entity base.</param>
        /// <param name="containerId"></param>
        /// <returns>Container.</returns>
        Container Create(PrimarySearchEntityBase primarySearchEntityBase, Guid containerId);
    }
}