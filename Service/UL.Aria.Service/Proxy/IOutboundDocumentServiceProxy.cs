using System.IO;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Proxy
{
	/// <summary>
	/// Interface IOutboundDocumentServiceProxy
	/// </summary>
	public interface IOutboundDocumentServiceProxy
	{
		/// <summary>
		/// Documents the exists.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool DocumentExists(OutboundDocumentDto outboundDocument);

		/// <summary>
		/// Saves the document.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="stream">The stream.</param>
		void SaveDocument(string metadata, Stream stream);
	}
}