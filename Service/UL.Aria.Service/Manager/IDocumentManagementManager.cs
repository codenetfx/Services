using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Interface IDocumentManagementManager
	/// </summary>
	public interface IDocumentManagementManager
	{
		/// <summary>
		/// Creates the existing document in the container with the primary search entity and links it to the task.
		/// </summary>
		/// <param name="documentId"></param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>
		/// Document Guid.
		/// </returns>
		Guid CreateAndLink(Guid documentId, Guid containerId, Guid taskId, IDictionary<string, string> metaData);
	}
}