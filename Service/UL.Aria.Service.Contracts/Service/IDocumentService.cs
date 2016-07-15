using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IDocumentService
	/// </summary>
	[ServiceContract]
	public interface IDocumentService
	{
		/// <summary>
		/// Creates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>System.String.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{containerId}", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		string Create(string containerId, IDictionary<string, string> metaData);
		
		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="metaData">The meta data.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Update(string id, IDictionary<string, string> metaData);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		IDictionary<string, string> FetchById(string id);

			/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "DELETE",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Delete(string id);

		/// <summary>
		/// Fetches the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/Document", Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentDto FetchDocumentById(string id);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Ping", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		string Ping();

		/// <summary>
		/// Locks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/Lock", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Lock(string id);

		/// <summary>
		/// Unlocks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/Unlock", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Unlock(string id);
	}
}