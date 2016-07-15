using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Interface for History Repository
    /// </summary>
    public interface IHistoryRepository
    {
        /// <summary>
        ///     Finds the history item by id.
        /// </summary>
        /// <param name="historyId">The history id.</param>
        /// <returns>History.</returns>
        History FindById(Guid historyId);

        /// <summary>
        ///     Finds all history items for a given Entity, i.e. Project History, Order History, etc
        /// </summary>
        /// <returns></returns>
        IEnumerable<History> FindAllByEntityId(Guid entityId);

        /// <summary>
        ///     Creates the specified history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>HistoryId.</returns>
        Guid Create(History history);

        /// <summary>
        ///     Updates the specified history item.
        /// </summary>
        /// <param name="history">The history item.</param>
        int Update(History history);

        /// <summary>
        ///     Deletes the specified history item.
        /// </summary>
        /// <param name="id">The history id.</param>
        void Delete(Guid id);
    }
}
