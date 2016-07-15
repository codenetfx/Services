using System;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// externalized representation of Configuration
    /// </summary>
    public interface IScratchSpaceConfigurationSource
    {
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <returns></returns>
        string this[Guid userid] { get; }

        /// <summary>
        /// Gets the expiration.
        /// </summary>
        /// <value></value>
		double Expiration { get; }
    }
}