using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IExternalDocumentService
	/// </summary>
	[ServiceContract(Namespace = "http://portal.ul.com", Name = "DocumentDetail")]
	public interface IExternalDocumentService
	{
		/// <summary>
		/// Saves the specified metadata.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>DocumentDto.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "", // "/{metadata}",
			Method = "PUT",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)] //,
			//BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[FaultContract(typeof(ArgumentException))]
		//[FaultContract(typeof(ArgumentNullException))]
		//[FaultContract(typeof(CommunicationException))]
		DocumentDto Save(Stream stream);

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/Ping",
			Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)] //,
			//BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[FaultContract(typeof(ArgumentException))]
		//[FaultContract(typeof(ArgumentNullException))]
		//[FaultContract(typeof(CommunicationException))]
		string Ping();

		/// <summary>
		/// Gets the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/{id}",
			Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentDto GetDocumentById(string id);
	}
}