using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using UL.Aria.Service.Contracts;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// Class DocumentContentService. This class cannot be inherited.
	/// </summary>
	[AutoRegisterRestServiceAttribute]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public sealed class DocumentContentService : IDocumentContentService
	{
		private readonly IDocumentContentManager _documentContentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentContentService" /> class.
		/// </summary>
		/// <param name="documentContentManager">The document content manager.</param>
		public DocumentContentService(IDocumentContentManager documentContentManager)
		{
			_documentContentManager = documentContentManager;
		}

		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		public void Create(string id, string contentType, Stream stream)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNullOrEmpty(contentType, "contentType");
			Guard.IsNotNull(stream, "stream");
			contentType = Encoding.UTF8.GetString(Convert.FromBase64String(contentType));

			_documentContentManager.Create(convertedId, contentType, stream, true);
		}

		internal string _testMetaData = null;

		/// <summary>
		/// Saves the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public void Save(Stream stream)
		{
			var metadata = WebOperationContext.Current == null ? _testMetaData : WebOperationContext.Current.IncomingRequest.Headers["metadata"];
			Guard.IsNotNullOrEmpty(metadata, "metadata");
			Guard.IsNotNull(stream, "stream");

			var convertedMetadata = Encoding.UTF8.GetString(Convert.FromBase64String(metadata));
			var inboundDocument = OutboundDocumentMetadata.ParseDocumentMetadata<InboundDocumentDto>(convertedMetadata);

			Guard.IsNotNullOrEmpty(inboundDocument.MessageId, "MessageId");
			Guard.IsNotEmptyGuid(inboundDocument.DocumentId, "DocumentId");

			_documentContentManager.Save(inboundDocument.DocumentId, inboundDocument.ContentType, stream);
		}

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		public Stream FetchById(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");

			return _documentContentManager.FetchById(convertedId);
		}

		/// <summary>
		/// This will create document templates and omits to assert provider call.Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		public void CreateDocumentTemplate(string id, string contentType, Stream stream)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNullOrEmpty(contentType, "contentType");
			Guard.IsNotNull(stream, "stream");
			contentType = Encoding.UTF8.GetString(Convert.FromBase64String(contentType));

			_documentContentManager.CreateDocumentTemplate(convertedId, contentType, stream);
		}

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="applyLock">if set to <c>true</c> [apply lock].</param>
		public void PrepareDocumentForEdit(string id, bool applyLock)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");

			_documentContentManager.PrepareDocumentForEdit(convertedId, applyLock);
		}
	}
}