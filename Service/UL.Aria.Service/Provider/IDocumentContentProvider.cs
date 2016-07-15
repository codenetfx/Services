using System;
using System.IO;

using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IDocumentContentProvider
	/// </summary>
	public interface IDocumentContentProvider
	{
		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		/// <param name="sendToDocumentEdi">if set to <c>true</c> [send to document edi].</param>
		/// <returns>BlobProperties.</returns>
		BlobProperties Create(Guid id, string contentType, Stream stream, bool sendToDocumentEdi);

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void PrepareDocumentForEdit(Guid id);

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		Stream FetchById(Guid id);
	}
}