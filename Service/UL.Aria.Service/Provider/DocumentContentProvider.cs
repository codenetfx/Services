using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Proxy;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class DocumentContentProvider. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentContentProvider : IDocumentContentProvider
	{
		private readonly IAzureBlobStorageRepository _azureBlobStorageRepository;
		private readonly ICryptographyProvider _cryptographyProvider;
		private readonly IAssetProvider _assetProvider;
		private readonly IOutboundDocumentServiceProxy _outboundDocumentServiceProxy;
		private readonly IAzureBlobStorageLocatorProvider _documentContentLocatorProvider;
		private readonly IDocumentContentProviderConfigurationSource _documentContentProviderConfigurationSource;
		private readonly IDocumentRepository _documentRepository;
		private readonly IDocumentVersionRepository _documentVersionRepository;
		private readonly IPrincipalResolver _principalResolver;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentContentProvider" /> class.
		/// </summary>
		/// <param name="documentContentProviderConfigurationSource">The document content provider configuration source.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="azureBlobStorageLocatorProviderResolver">The azure BLOB storage locator provider resolver.</param>
		/// <param name="azureBlobStorageRepository">The azure BLOB storage repository.</param>
		/// <param name="documentVersionRepository">The document version repository.</param>
		/// <param name="documentRepository">The document repository.</param>
		/// <param name="cryptographyProvider">The cryptography provider.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="outboundDocumentServiceProxy">The outbound document service proxy.</param>
		public DocumentContentProvider(IDocumentContentProviderConfigurationSource documentContentProviderConfigurationSource,
			IPrincipalResolver principalResolver, ITransactionFactory transactionFactory,
			IAzureBlobStorageLocatorProviderResolver azureBlobStorageLocatorProviderResolver,
			IAzureBlobStorageRepository azureBlobStorageRepository, IDocumentVersionRepository documentVersionRepository,
			IDocumentRepository documentRepository, ICryptographyProvider cryptographyProvider,
			IAssetProvider assetProvider,
			IOutboundDocumentServiceProxy outboundDocumentServiceProxy)
		{
			_documentContentProviderConfigurationSource = documentContentProviderConfigurationSource;
			_principalResolver = principalResolver;
			_transactionFactory = transactionFactory;
			_documentContentLocatorProvider = azureBlobStorageLocatorProviderResolver.Resolve("DocumentContent");
			_azureBlobStorageRepository = azureBlobStorageRepository;
			_documentVersionRepository = documentVersionRepository;
			_documentRepository = documentRepository;
			_cryptographyProvider = cryptographyProvider;
			_assetProvider = assetProvider;
			_outboundDocumentServiceProxy = outboundDocumentServiceProxy;
		}

		/// <summary>
		/// Stores the document in the stream by the specified identifier with the content type.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Document content type.</param>
		/// <param name="stream">The document stream.</param>
		/// <param name="sendToDocumentEdit">if set to <c>true</c> [send to document edit].</param>
		public BlobProperties Create(Guid id, string contentType, Stream stream, bool sendToDocumentEdit)
		{
			BlobProperties blobProperties;

			var azureBlobStorageConfiguration = _documentContentLocatorProvider.FetchById(id);

			using (var transactionScope = _transactionFactory.Create())
			{
				// create the location for the document de dup's document content if it does not exist
				_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);

				// store the document de dup's document content
				var documentVersionId = Guid.NewGuid();
				using (var cryptStream = new CryptoStream(stream,
					_cryptographyProvider.CreateEncryptor(),
					CryptoStreamMode.Read))
				{
					blobProperties = _azureBlobStorageRepository.Save(azureBlobStorageConfiguration, null, documentVersionId.ToString(),
						new BlobProperties {ContentType = contentType}, cryptStream);
				}

				// calculate hash
				string hashValue;
				using (var streamRead =
					_azureBlobStorageRepository.Fetch(azureBlobStorageConfiguration, documentVersionId.ToString()).Stream)
				{
					var streamReadDecrypted = new CryptoStream(streamRead, _cryptographyProvider.CreateDecryptor(),
						CryptoStreamMode.Read);
					HashAlgorithm hash = new SHA256CryptoServiceProvider();
					var bytes = hash.ComputeHash(streamReadDecrypted);
					hashValue = BitConverter.ToString(bytes).Replace("-", string.Empty);
				}

				// if the document de dup exists get it and remove the corresponding document content, otherwise create a document de dup
				var createdDateTime = DateTime.UtcNow;
				DocumentVersion documentVersion;
				try
				{
					documentVersion = _documentVersionRepository.GetDocumentVersionByHashValue(hashValue);
					_azureBlobStorageRepository.Delete(azureBlobStorageConfiguration, documentVersionId.ToString());
				}
				catch (DatabaseItemNotFoundException)
				{
					documentVersion = new DocumentVersion(documentVersionId)
					{
						HashValue = hashValue,
						CreatedById = _principalResolver.UserId,
						CreatedDateTime = createdDateTime,
						UpdatedById = _principalResolver.UserId,
						UpdatedDateTime = createdDateTime
					};
					_documentVersionRepository.Create(documentVersion);
				}

				// if the document exists update the document's de dup id if it has changed, otherwise if the document does not exist create it
				Document document;
				try
				{
					document = _documentRepository.FindById(id);
					if (document.DocumentVersionId != documentVersion.Id.GetValueOrDefault())
					{
						document.DocumentVersionId = documentVersion.Id.GetValueOrDefault();
						_documentRepository.Update(document);
					}
				}
				catch (DatabaseItemNotFoundException)
				{
					document = new Document(id)
					{
						DocumentVersionId = documentVersion.Id.GetValueOrDefault(),
						CreatedById = _principalResolver.UserId,
						CreatedDateTime = createdDateTime,
						UpdatedById = _principalResolver.UserId,
						UpdatedDateTime = createdDateTime
					};
					_documentRepository.Create(document);
				}

				transactionScope.Complete();
			}

			if (sendToDocumentEdit)
			{
				PrepareDocumentForEdit(id);
			}

			blobProperties.Uri = new Uri(_documentContentProviderConfigurationSource.DocumentServiceUri.AbsoluteUri + id);

			return blobProperties;
		}

		internal static void CheckDocumentLocked(IDictionary<string, string> assetMetadata)
		{
			if (assetMetadata.ContainsKey(AssetFieldNames.AriaLockedBy) &&
				!string.IsNullOrWhiteSpace(assetMetadata[AssetFieldNames.AriaLockedBy]))
			{
				throw new InvalidOperationException(string.Format("Document is locked by {0}", assetMetadata[AssetFieldNames.AriaLockedBy]));
			}
		}

		internal static void CheckDocumentLocked(IDictionary<string, string> assetMetadata, string userName)
		{
			if (assetMetadata.ContainsKey(AssetFieldNames.AriaLockedBy) &&
				!string.IsNullOrWhiteSpace(assetMetadata[AssetFieldNames.AriaLockedBy]) && assetMetadata[AssetFieldNames.AriaLockedBy] != userName)
			{
				throw new InvalidOperationException(string.Format("Document is locked by {0}", assetMetadata[AssetFieldNames.AriaLockedBy]));
			}
		}

		/// <summary>
		/// Prepares the document for edit.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void PrepareDocumentForEdit(Guid id) //, string contentType)
		{
			if (_documentContentProviderConfigurationSource.IsOutboundDocumentEnabled)
			{
				Document document;
				IDictionary<string, string> assetMetadata;

				using (var transactionScope = _transactionFactory.Create())
				{
					document = _documentRepository.FindById(id);
					assetMetadata = _assetProvider.Fetch(id);
					transactionScope.Complete();
				}

				CheckDocumentLocked(assetMetadata, _principalResolver.Current.Identity.Name);

				var outboundDocumentDto = new OutboundDocumentDto
				{
					MessageId = Guid.NewGuid().ToString(),
					DocumentId = id,
					ContentType = assetMetadata.ContainsKey(AssetFieldNames.AriaContentType) ? assetMetadata[AssetFieldNames.AriaContentType] : "", //contentType,
					Description = assetMetadata.ContainsKey(AssetFieldNames.AriaProductDescription) ? assetMetadata[AssetFieldNames.AriaProductDescription] : "",
					DocumentTypeId = assetMetadata.ContainsKey(AssetFieldNames.AriaDocumentTypeId) ? new Guid(assetMetadata[AssetFieldNames.AriaDocumentTypeId]) : (Guid?)null,
					LastModifiedBy = assetMetadata.ContainsKey(AssetFieldNames.AriaLastModifiedBy) ? assetMetadata[AssetFieldNames.AriaLastModifiedBy] : "",
					OriginalFileName = assetMetadata.ContainsKey(AssetFieldNames.AriaName) ? assetMetadata[AssetFieldNames.AriaName] : "",
					Permission = assetMetadata.ContainsKey(AssetFieldNames.AriaPermission) ? (DocumentPermissionEnumDto)Enum.Parse(typeof(DocumentPermissionEnumDto), assetMetadata[AssetFieldNames.AriaPermission]) : DocumentPermissionEnumDto.Private,
					Size = assetMetadata.ContainsKey(AssetFieldNames.AriaSize) ? Convert.ToInt32(assetMetadata[AssetFieldNames.AriaSize]) : 0,
					Title = assetMetadata.ContainsKey(AssetFieldNames.AriaTitle) ? assetMetadata[AssetFieldNames.AriaTitle] : "",
					HashValue = document.HashValue
				};
				var fileExists = _outboundDocumentServiceProxy.DocumentExists(outboundDocumentDto);
				if (!fileExists)
				{
					var outboundDocumentXml = Serialize(outboundDocumentDto);
					var bytes = Encoding.UTF8.GetBytes(outboundDocumentXml);
					var encodedString = Convert.ToBase64String(bytes);
					var editStream = FetchById(outboundDocumentDto.DocumentId);
					_outboundDocumentServiceProxy.SaveDocument(encodedString, editStream);
				}
			}
		}

		private static string Serialize<T>(T value)
		{
			if (value == null) return string.Empty;

			var xmlSerializer = new XmlSerializer(typeof(T));

			using (var stringWriter = new StringWriter())
			{
				using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
				{
					xmlSerializer.Serialize(writer, value);
					return stringWriter.ToString();
				}
			}
		}

		/// <summary>
		/// Fetches the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Stream.</returns>
		public Stream FetchById(Guid id)
		{
			var document = _documentRepository.FindById(id);
			var azureBlobStorageConfiguration = _documentContentLocatorProvider.FetchById(id);
			var stream =
				_azureBlobStorageRepository.Fetch(azureBlobStorageConfiguration, document.DocumentVersionId.ToString()).Stream;
			return new CryptoStream(stream, _cryptographyProvider.CreateDecryptor(), CryptoStreamMode.Read);
		}
	}
}