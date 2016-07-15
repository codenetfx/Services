using System;
using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     contract for coordinating history oriented funtionality
    /// </summary>
    public interface IHistoryManager
    {
        /// <summary>
        ///     Gets history items by entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>IEnumerable{History}</returns>
        IEnumerable<History> GetHistoryByEntityId(Guid entityId);

        /// <summary>
        /// Creates a new history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>historyId</returns>
        Guid Create(History history);

		/// <summary>
		/// Downloads the history by entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Stream.</returns>
	    Stream DownloadHistoryByEntityId(Guid entityId);

		/// <summary>
		/// Downloads the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
	    Stream DownloadTaskHistory(Guid id, Guid containerId);

		/// <summary>
		/// Fetches the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
	    IEnumerable<History> FetchTaskHistory(Guid id, Guid containerId);
    }
}