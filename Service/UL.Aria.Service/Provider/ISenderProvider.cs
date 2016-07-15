using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface ISenderProvider
    /// </summary>
    public interface ISenderProvider
    {
        /// <summary>
        ///     Creates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void Create(Sender sender);

        /// <summary>
        ///     Finds the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Sender.</returns>
        Sender FindByName(string name);
    }
}