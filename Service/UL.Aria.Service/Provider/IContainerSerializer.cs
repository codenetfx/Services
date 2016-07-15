using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IContainerSerializer
    /// </summary>
    public interface IContainerSerializer
    {
        /// <summary>
        ///     Serializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>System.String.</returns>
        string Serialize(Container container);
    }
}