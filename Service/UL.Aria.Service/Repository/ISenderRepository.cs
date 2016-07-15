using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Interface ISenderRepository
    /// </summary>
    public interface ISenderRepository
    {
        /// <summary>
        ///     Fetches the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Sender.</returns>
        Sender FetchByName(string name);

        /// <summary>
        ///     Creates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void Create(Sender sender);
    }
}