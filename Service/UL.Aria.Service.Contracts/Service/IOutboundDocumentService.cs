using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IOutboundDocumentService
	/// </summary>
	[ServiceContract]
	public interface IOutboundDocumentService
	{
		/// <summary>
		/// Documents the exists.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/DocumentExists",
			Method = "POST",
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		bool DocumentExists(OutboundDocumentDto outboundDocument);

		/// <summary>
		/// Saves the document.
		/// </summary>
		/// <param name="stream">The stream.</param>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "",
			Method = "PUT",
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml)]
		void SaveDocument(Stream stream);
	}
}