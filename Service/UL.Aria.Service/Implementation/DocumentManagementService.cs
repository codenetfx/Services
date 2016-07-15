using System.Collections.Generic;
using System.ServiceModel;

using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// Class DocumentManagementService.
	/// </summary>
	[AutoRegisterRestService]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class DocumentManagementService : IDocumentManagementService
	{
		private readonly IDocumentManagementManager _documentManagementManager;
		// ReSharper disable once InconsistentNaming
		internal string _testMetaData = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentManagementService" /> class.
		/// </summary>
		public DocumentManagementService(IDocumentManagementManager documentManagementManager)
		{
			_documentManagementManager = documentManagementManager;
		}

		/// <summary>
		/// Creates the existing document in the container with the primary search entity and links it to the task.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>The document Id, System.String.</returns>
		public string CreateAndLink(string documentId, string containerId, string taskId,
			IDictionary<string, string> metaData)
		{
			Guard.IsNotNullOrEmpty(documentId, "documentId");
			var convertedDocumentId = documentId.ToGuid();
			Guard.IsNotEmptyGuid(convertedDocumentId, "documentId");
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = containerId.ToGuid();
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");
			Guard.IsNotNullOrEmpty(taskId, "taskId");
			var convertedTaskId = taskId.ToGuid();
			Guard.IsNotEmptyGuid(convertedTaskId, "taskId");
			Guard.IsNotNull(metaData, "metaData");

			var newDocumentId = _documentManagementManager.CreateAndLink(convertedDocumentId, convertedContainerId,
				convertedTaskId, metaData);

			return newDocumentId.ToString();
		}
	}
}