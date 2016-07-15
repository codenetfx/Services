using System;
using System.IO;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Class DocumentContentManager. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentContentManager : IDocumentContentManager
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IDocumentManager _documentManager;
		private readonly IDocumentContentProvider _documentContentProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentContentManager" /> class.
		/// </summary>
		/// <param name="documentContentProvider">The document content provider.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="documentManager">The document manager.</param>
		public DocumentContentManager(IDocumentContentProvider documentContentProvider, IAssetProvider assetProvider, IDocumentManager documentManager)
		{
			_documentContentProvider = documentContentProvider;
			_assetProvider = assetProvider;
			_documentManager = documentManager;
		}

		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		/// <param name="sendToDocumentEdit">if set to <c>true</c> [send to document edit].</param>
		/// <returns>Uri, the document Uri.</returns>
		public Uri Create(Guid id, string contentType, Stream stream, bool sendToDocumentEdit)
		{
			var assetMetadata = _assetProvider.Fetch(id);
			DocumentContentProvider.CheckDocumentLocked(assetMetadata);
			var blobProperties = _documentContentProvider.Create(id, contentType, stream, sendToDocumentEdit);
			_assetProvider.UpdateContentUriAndSize(id, blobProperties.Uri, blobProperties.Size);
			return blobProperties.Uri;
		}

		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="stream">The stream.</param>
		public void Save(Guid id, string contentType, Stream stream)
		{
			var blobProperties = _documentContentProvider.Create(id, contentType, stream, false);
			_assetProvider.UpdateContentUriAndSize(id, blobProperties.Uri, blobProperties.Size);
		}

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		public Stream FetchById(Guid id)
		{
			return _documentContentProvider.FetchById(id);
		}


		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		/// <returns>Uri, the document Uri.</returns>
		public Uri CreateDocumentTemplate(Guid id, string contentType, Stream stream)
		{
			var blobProperties = _documentContentProvider.Create(id, contentType, stream, false);
			return blobProperties.Uri;
		}

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="applyLock">if set to <c>true</c> [apply lock].</param>
		public void PrepareDocumentForEdit(Guid id, bool applyLock)
		{
			_documentContentProvider.PrepareDocumentForEdit(id);

			if (applyLock)
			{
				_documentManager.Lock(id);
			}
		}
	}
}