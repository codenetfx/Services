using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Interface for History Provider
    /// </summary>
    public interface IHistoryProvider
    {
        /// <summary>
        ///     Fetches all history items for a given Entity, i.e. Project History, Order History, etc
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>IEnumerable{History}.</returns>
        IEnumerable<History> FetchHistoryByEntityId(Guid entityId);

        /// <summary>
        ///     Creates the specified history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>HistoryId.</returns>
        Guid Create(History history);
    }
}
