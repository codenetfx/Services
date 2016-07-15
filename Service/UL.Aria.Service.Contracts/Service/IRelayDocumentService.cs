using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IRelayDocumentService
	/// </summary>
	[ServiceContract(Namespace = "http://portal.ul.com")]
	public interface IRelayDocumentService
	{
		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>DocumentDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "", Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentDto Save(Stream stream);

		/// <summary>
		/// Gets the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		[OperationContract]
		[WebInvoke(
			Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		DocumentDto GetDocumentById(string id);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Ping", Method = "POST",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		string Ping();
	}
}