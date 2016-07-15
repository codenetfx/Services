using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IDocumentManagementService
	/// </summary>
	[ServiceContract]
	public interface IDocumentManagementService
	{
		/// <summary>
		/// Creates the existing document in the container with the primary search entity and links it to the task.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>The document Id, System.String.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate =
				"/{documentId}?operation=CreateAndLink&containerId={containerId}&taskId={taskId}",
			Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		string CreateAndLink(string documentId, string containerId, string taskId,
			IDictionary<string, string> metaData);
	}
}