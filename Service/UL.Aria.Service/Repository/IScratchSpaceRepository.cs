using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// interface that abstracts the storage medium
    /// </summary>
    public interface IScratchSpaceRepository
    {
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<ScratchFileInfo> FetchAll(Guid userId);

        /// <summary>
        /// Fetches the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        Stream FetchContent(Guid userId, Guid fileId);

        /// <summary>
        /// Publishes the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentStream">The content stream.</param>
        /// <returns></returns>
        Guid PublishContent(Guid userId, string fileName, Stream contentStream);

	    /// <summary>
	    /// Purges the specified user's files
	    /// </summary>
		/// <param name="userid">The userid.</param>
		/// <param name="forceDelete">True to delete all files regardless of age</param>
	    void Purge(Guid userid, bool forceDelete);
    }
}