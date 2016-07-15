using System;
using System.IO;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Interface IDocumentContentManager
	/// </summary>
	public interface IDocumentContentManager
	{
		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		/// <param name="sendToDocumentEdi">if set to <c>true</c> [send to document edi].</param>
		/// <returns>Uri, the document Uri.</returns>
		Uri Create(Guid id, string contentType, Stream stream, bool sendToDocumentEdi);

		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="stream">The stream.</param>
		void Save(Guid id, string contentType, Stream stream);

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		Stream FetchById(Guid id);


		/// <summary>
		/// Creates the document template.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		Uri CreateDocumentTemplate(Guid id, string contentType, Stream stream);

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="applyLock">if set to <c>true</c> [apply lock].</param>
		void PrepareDocumentForEdit(Guid id, bool applyLock);
	}
}