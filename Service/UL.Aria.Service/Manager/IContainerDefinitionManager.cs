using System;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Interface IContainerDefinitionManager
    /// </summary>
    public interface IContainerDefinitionManager
    {
        /// <summary>
        ///     Creates the template.
        /// </summary>
        /// <returns>System.String.</returns>
        string Create(Guid containerId, Guid? companyId, string containerType);
    }
}