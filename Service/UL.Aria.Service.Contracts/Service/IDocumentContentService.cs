using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IDocumentContentService
	/// </summary>
	[ServiceContract]
	public interface IDocumentContentService
	{
		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/{contentType}", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Create(string id, string contentType, Stream stream);

		/// <summary>
		/// Saves the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Save(Stream stream);

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		Stream FetchById(string id);



		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="documentId">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "DocumentTemplate/{documentId}/{contentType}", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void CreateDocumentTemplate(string documentId, string contentType, Stream stream);

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="applyLock">if set to <c>true</c> [apply lock].</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/PrepareDocumentForEdit?applyLock={applyLock}", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void PrepareDocumentForEdit(string id, bool applyLock);
	}
}