using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.SharePoint;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Asset provider implementation for content(assets in SharePoint) like documents and tasks.
	/// </summary>
	public sealed class AssetProvider : IAssetProvider
	{
		/// <summary>
		/// The claim schema
		/// </summary>
		public const string ClaimSchema = "http://aria.ul.com/CompoundClaim";

		/// <summary>
		/// The claim provider
		/// </summary>
		public const string ClaimProvider = "ClaimProvider:AriaClaimProvider";

		/// <summary>
		/// The permission key
		/// </summary>
		public const string PermissionKey = "ariaPermission";

		private readonly IAriaMetaDataLinkRepository _ariaMetaDataLinkRepository;
		private readonly IAriaMetaDataRepository _ariaMetaDataRepository;
		private readonly IAssetFieldMetadata _assetFieldMetaData;
		private readonly IContainerManager _containerManager;
		private readonly IPrincipalResolver _principalResolver;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssetProvider" /> class.
		/// </summary>
		/// <param name="containerManager">The container manager.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="assetFieldMetaData">The asset field meta data.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="ariaMetaDataRepository">The aria meta data repository.</param>
		/// <param name="ariaMetaDataLinkRepository">The aria meta data link repository.</param>
		public AssetProvider(IContainerManager containerManager,
			ITransactionFactory transactionFactory,
			IAssetFieldMetadata assetFieldMetaData, IPrincipalResolver principalResolver,
			IAriaMetaDataRepository ariaMetaDataRepository, IAriaMetaDataLinkRepository ariaMetaDataLinkRepository)
		{
			_containerManager = containerManager;
			_transactionFactory = transactionFactory;
			_assetFieldMetaData = assetFieldMetaData;
			_principalResolver = principalResolver;
			_ariaMetaDataRepository = ariaMetaDataRepository;
			_ariaMetaDataLinkRepository = ariaMetaDataLinkRepository;
		}

		/// <summary>
		/// Fetches all documents.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public SearchResultSet FetchAllDocuments(Guid containerId)
		{
			var results = FetchAllAssets(containerId, EntityTypeEnumDto.Document);
			if (!_principalResolver.Current.HasClaim(SecuredClaims.UlEmployee, SecuredActions.Role))
			{
				results.Results =
					results.Results.Where(
						x =>
							(x.Metadata.ContainsKey(AssetFieldNames.AriaPermission) &&
							 (x.Metadata[AssetFieldNames.AriaPermission] == "Modify"
							  || x.Metadata[AssetFieldNames.AriaPermission] == "ReadOnly"
								 )
								)).ToList();
				results.Summary =
					new SearchSummary
					{
						TotalResults = results.Results.Count(),
						StartIndex = 0,
						EndIndex = results.Results.Count() - 1,
						LastCommand = containerId.ToString()
					};
			}
			return results;
		}

		/// <summary>
		///     Fetches all assets in a container.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="assetType"></param>
		/// <returns>SearchResultSet.</returns>
		public SearchResultSet FetchAllAssets(Guid containerId, EntityTypeEnumDto? assetType = null)
		{
			var ariaMetaDataItems = _ariaMetaDataRepository.FetchByParentId(containerId);
			var searchResultSet = new SearchResultSet
			{
				Results = new List<SearchResult>()
			};
			foreach (var ariaMetaDataItem in ariaMetaDataItems)
			{
				var metaData = ariaMetaDataItem.MetaData.ToIDictionaryFromAriaSharepointXml();
				metaData[AssetFieldNames.SharePointAssetId] = ariaMetaDataItem.AssetId.ToString();
				var searchResult = new SearchResult
				{
					Id = ariaMetaDataItem.AssetId,
					Metadata = metaData,
					EntityType = metaData.GetValue(AssetFieldNames.AriaAssetType, EntityTypeEnumDto.Container),
					ChangeDate = metaData.GetValue(AssetFieldNames.AriaLastModifiedOn, default(DateTime)),
					Title = metaData.GetValue(AssetFieldNames.AriaTitle, default(string)),
					Name = metaData.GetValue(AssetFieldNames.AriaName, default(string))
				};
				if (null == assetType || searchResult.EntityType == assetType)
					searchResultSet.Results.Add(searchResult);
			}
			searchResultSet.Summary =
				new SearchSummary
				{
					TotalResults = searchResultSet.Results.Count(),
					StartIndex = 0,
					EndIndex = searchResultSet.Results.Count() - 1,
					LastCommand = containerId.ToString()
				};
			return searchResultSet;
		}

		/// <summary>
		///     Fetches entity content.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <returns>The found content.</returns>
		public Stream FetchContent(Guid assetId)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     Fetches entity meta data.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <returns>The found meta data.</returns>
		public IDictionary<string, string> Fetch(Guid assetId)
		{
			var ariaMetaData = _ariaMetaDataRepository.FetchById(assetId);
			return ariaMetaData.MetaData.ToIDictionaryFromAriaSharepointXml();
		}

		/// <summary>
		///     Creates entity content.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <param name="contentStream">The content stream.</param>
		/// <returns>The created content id.</returns>
		public string CreateContent(Guid assetId, Stream contentStream)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     Creates entity metadata.
		/// </summary>
		/// <param name="containerId"></param>
		/// <param name="metadataStream">The metadata stream.</param>
		/// <param name="newEntityId"></param>
		/// <returns>The created content id.</returns>
		public string Create(Guid containerId, IDictionary<string, string> metadataStream, Guid newEntityId)
		{
			metadataStream[AssetFieldNames.AriaContainerId] = containerId.ToString();
			var metadataXml = metadataStream.ToAriaSharepointXml(_assetFieldMetaData);
			return CreateAsset(containerId, newEntityId, metadataXml).ToString();
		}

		/// <summary>
		///     Creates entity metadata.
		/// </summary>
		/// <param name="newContainerId"></param>
		/// <param name="newAssetId"></param>
		/// <param name="primarySearchEntityBase">The container.</param>
		/// <returns>The created content id.</returns>
		public Guid Create(Guid newContainerId, Guid newAssetId, PrimarySearchEntityBase primarySearchEntityBase)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				CreateNoTransaction(newContainerId, newAssetId, primarySearchEntityBase);

				transactionScope.Complete();
			}

			return newContainerId;
		}

		// Always call from an existing transaction not meant to be used out of transaction ever
		private void CreateNoTransaction(Guid newContainerId, Guid newAssetId, PrimarySearchEntityBase primarySearchEntityBase)
		{
			if (primarySearchEntityBase.CreateContainer)
				_containerManager.Create(primarySearchEntityBase, newContainerId);
			// Sets the primarySearchEntityBase.ContainerId to container's db id in this routine
			else
				primarySearchEntityBase.ContainerId = newContainerId;

			using (var stream = primarySearchEntityBase.GetAssetMetadata(_assetFieldMetaData))
			{
				var metaDataXml = StreamToString(stream);
				CreateAsset(newContainerId, newAssetId, metaDataXml);
			}
		}

		/// <summary>
		///     Updates entity content.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <param name="contentStream">The content stream.</param>
		public void UpdateContent(Guid assetId, Stream contentStream)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Updates the content's uri and size.
		/// </summary>
		/// <param name="assetId">The asset identifier.</param>
		/// <param name="contentUri">The content URI.</param>
		/// <param name="size">The size.</param>
		public void UpdateContentUriAndSize(Guid assetId, Uri contentUri, long size)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var ariaMetaData = _ariaMetaDataRepository.FetchById(assetId);
				ariaMetaData.Uri = contentUri.OriginalString;
				var metaDataDictionary = ariaMetaData.MetaData.ToIDictionaryFromAriaSharepointXml();
				metaDataDictionary[AssetFieldNames.AriaSize] = size.ToString();
				ariaMetaData.MetaData = metaDataDictionary.ToAriaSharepointXml(_assetFieldMetaData);
				UpdateAsset(_ariaMetaDataRepository, ariaMetaData);
				transactionScope.Complete();
			}
		}

		internal void ValidateDocumentNameIsUniqueForProject(Guid containerId, Guid documentId, string documentName, string documentPermission)
		{
			var documentSearchResults = FetchAllDocuments(containerId);
			foreach (var documentSearchResult in documentSearchResults.Results)
			{
				var documentSearchResultId = new Guid(documentSearchResult.Metadata[AssetFieldNames.SharePointAssetId]);
				if (documentSearchResultId != documentId)
				{
					if (string.Equals(documentName, documentSearchResult.Metadata[AssetFieldNames.AriaTitle], StringComparison.CurrentCultureIgnoreCase) && documentPermission == documentSearchResult.Metadata[AssetFieldNames.AriaPermission])
					{
						throw new InvalidOperationException(string.Format("A file with the same Name '{0}' and Customer Access '{1}' already exists.", documentName, documentPermission));
					}
				}
			}
		}

		/// <summary>
		///     Updates entity metadata.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <param name="metadataStream">The metadata stream.</param>
		public void Update(Guid assetId, IDictionary<string, string> metadataStream)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var ariaMetaData = _ariaMetaDataRepository.FetchById(assetId);
				var metaDataDictionary = ariaMetaData.MetaData.ToIDictionaryFromAriaSharepointXml();
				var containerId = metaDataDictionary[AssetFieldNames.AriaContainerId].ToGuid();
				ValidateDocumentNameIsUniqueForProject(containerId, assetId, metadataStream[AssetFieldNames.AriaTitle], metadataStream[AssetFieldNames.AriaPermission]);
				metadataStream[AssetFieldNames.AriaContainerId] = containerId.ToString();
				ariaMetaData.MetaData = metadataStream.ToAriaSharepointXml(_assetFieldMetaData);
				UpdateAsset(_ariaMetaDataRepository, ariaMetaData);

				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Updates entity metadata.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <param name="primarySearchEntityBase">The container.</param>
		public void Update(Guid assetId, PrimarySearchEntityBase primarySearchEntityBase)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var ariaMetaData = _ariaMetaDataRepository.FetchById(assetId);
				UpdateNoTransaction(ariaMetaData, primarySearchEntityBase);

				transactionScope.Complete();
			}
		}

		// Always call from an existing transaction not meant to be used out of transaction ever
		private void UpdateNoTransaction(AriaMetaData ariaMetaData, PrimarySearchEntityBase primarySearchEntityBase)
		{
			var metaDataDictionary = ariaMetaData.MetaData.ToIDictionaryFromAriaSharepointXml();
			var containerId = metaDataDictionary[AssetFieldNames.AriaContainerId].ToGuid();

			if (primarySearchEntityBase.CreateContainer)
				_containerManager.Update(primarySearchEntityBase, containerId);
			// Sets the primarySearchEntityBase.ContainerId to container's db id in this routine
			else
				primarySearchEntityBase.ContainerId = containerId;

			using (var stream = primarySearchEntityBase.GetAssetMetadata(_assetFieldMetaData))
			{
				ariaMetaData.MetaData = StreamToString(stream);
				UpdateAsset(_ariaMetaDataRepository, ariaMetaData);
			}
		}

		/// <summary>
		///     Deletes the content.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		public void DeleteContent(Guid assetId)
		{
			Delete(assetId);
		}

		/// <summary>
		/// Creates the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		public void CreateAssetLink(AssetLink assetLink)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_ariaMetaDataLinkRepository.Create(assetLink.ParentAssetId, assetLink.AssetId);
				_ariaMetaDataRepository.ReCrawlAsset(assetLink.AssetId);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Deletes the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		public void DeleteAssetLink(AssetLink assetLink)
		{
		    using (var transactionScope = _transactionFactory.Create())
		    {
                _ariaMetaDataLinkRepository.Delete(assetLink.ParentAssetId, assetLink.AssetId);
                _ariaMetaDataRepository.ReCrawlAsset(assetLink.AssetId);
                transactionScope.Complete();;
		    }
		    
		}

		/// <summary>
		///     Fetches the asset links.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <returns>IList{System.String}.</returns>
		public IList<string> FetchAssetLinks(Guid assetId)
		{
			return _ariaMetaDataLinkRepository.FetchAssetLinks(assetId).Select(x => x.ParentId.ToString()).ToList();
		}

		/// <summary>
		///     Fetches the parent asset links.
		/// </summary>
		/// <param name="parentId">The parent id.</param>
		/// <returns>System.Collections.Generic.IList{System.String}.</returns>
		public IList<string> FetchParentAssetLinks(Guid parentId)
		{
			return _ariaMetaDataLinkRepository.FetchParentLinks(parentId).Select(x => x.AssetId.ToString()).ToList();
		}

		/// <summary>
		///     Deletes the specified asset id.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		public void Delete(Guid assetId)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_ariaMetaDataRepository.Delete(assetId);

				var ariaMetaDataLinks = _ariaMetaDataLinkRepository.FetchAssetLinks(assetId);
				foreach (var ariaMetaDataLink in ariaMetaDataLinks)
				{
					_ariaMetaDataLinkRepository.Delete(ariaMetaDataLink.ParentId, assetId);
				}

				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Fetches the multiple parent asset links.
		/// </summary>
		/// <param name="parentIds">The parent ids.</param>
		/// <returns>IEnumerable{MetaDataLink}.</returns>
		public IEnumerable<MetaDataLink> FetchMultipleParentAssetLinks(IEnumerable<Guid> parentIds)
		{
			var metaDataLinks = new List<MetaDataLink>();
			var parentIdsList = parentIds.ToList();

			foreach (var parentId in parentIdsList)
			{
				var parentMetaDataLinks = _ariaMetaDataLinkRepository.FetchParentLinks(parentId);

				foreach (var parentMetaDataLink in parentMetaDataLinks)
				{
					parentMetaDataLink.ParentId = parentId;
					metaDataLinks.Add(parentMetaDataLink);
				}
			}

			return metaDataLinks;
		}

		/// <summary>
		/// Saves the assets.
		/// </summary>
		/// <param name="entities">The entities.</param>
		public void SaveAssets(IEnumerable<PrimarySearchEntityBase> entities)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				foreach (var entity in entities)
				{
					var assetId = entity.Id.GetValueOrDefault();
					AriaMetaData ariaMetData = null;

					try
					{
						ariaMetData = _ariaMetaDataRepository.FetchById(assetId);
					}
					catch (DatabaseItemNotFoundException)
					{
					}

					if (ariaMetData == null)
					{
						var containerId = entity.ContainerId.GetValueOrDefault();
						CreateNoTransaction(containerId, assetId, entity);
					}
					else
					{
						UpdateNoTransaction(ariaMetData, entity);
					}
				}

				transactionScope.Complete();
			}
		}

		internal static void UpdateAsset(IAriaMetaDataRepository ariaMetaDataRepository, AriaMetaData ariaMetaData)
		{
			if (ariaMetaData.ParentAssetId != Guid.Empty)
			{
				var newMetaData = ParseAllMetadataToDictionary(ariaMetaData.MetaData);
				ariaMetaData.Claims = GetContainerDefinitionClaims(ariaMetaDataRepository, ariaMetaData.ParentAssetId, newMetaData);
			}
			ariaMetaData.LastModifiedTime = DateTime.UtcNow;
			ariaMetaData.IsParsed = false;
			ariaMetaDataRepository.Update(ariaMetaData);
		}

		private Guid CreateAsset(Guid containerId, Guid assetId, string metaDataXml)
		{
			if (assetId == Guid.Empty)
			{
				assetId = Guid.NewGuid();
			}

			var metadataDictionary = ParseAllMetadataToDictionary(metaDataXml);
			if (metadataDictionary == null || metadataDictionary.Count == 0)
			{
				//LoggingService.Instance.LogError("PersistenceManager::CreateAsset() Invalid Metadata Dictionary");
				throw new InvalidOperationException("Invalid metadata for CreateAsset. No values specified.");
			}

			var claims = CompoundClaimStringForSearch(ClaimSchema, ClaimProvider,
				metadataDictionary);

			if (string.IsNullOrWhiteSpace(claims))
			{
				claims = GetContainerDefinitionClaims(_ariaMetaDataRepository, containerId, metadataDictionary);
			}

			_ariaMetaDataRepository.Create(new AriaMetaData
			{
				Id = assetId,
				ParentAssetId = containerId,
				AssetName = string.Empty,
				MetaData = metaDataXml,
				LastModifiedTime = DateTime.UtcNow,
				Claims = claims,
				SecurityDescriptor = null,
				Uri = string.Empty,
				Version = string.Empty,
				IsParsed = false,
				IsDeleted = false
			});

			return assetId;
		}

		/// <summary>
		/// Parses all metadata to dictionary.
		/// </summary>
		/// <param name="metaData">The meta data.</param>
		/// <returns>Dictionary&lt;System.String, MetaDataTypeAndValue&gt;.</returns>
		/// <exception cref="System.Exception"></exception>
		/// <exception cref="System.InvalidOperationException"></exception>
		public static Dictionary<string, MetaDataTypeAndValue> ParseAllMetadataToDictionary(string metaData)
		{
			Dictionary<string, MetaDataTypeAndValue> metaDataDictionary;
			try
			{
				metaDataDictionary = Parse(metaData);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Failed to parse metadata:  {0}", metaData), ex);
			}

			foreach (var property in metaDataDictionary)
			{
				if (property.Key.Length > 64)
				{
					//LoggingService.Instance.LogError("MetadataManager::ParseAllMetadataToDictionary failed. Property {0} length > 64 characters.", property.Key);
					throw new InvalidOperationException(
						string.Format("MetadataManager::ParseAllMetadataToDictionary failed. Property {0} length > 64 characters.",
							property.Key));
				}
			}
			return metaDataDictionary;
		}

		private static Dictionary<string, MetaDataTypeAndValue> Parse(string metaData)
		{
			Guard.IsNotNullOrEmpty(metaData, "metaData");

			var metaDataDictionary = new Dictionary<string, MetaDataTypeAndValue>();
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(metaData.Trim());
			var xmlNodes = xmlDoc.SelectNodes("descendant::*");
			if (xmlNodes != null)
			{
				foreach (var xmlNode in xmlNodes)
				{
					var xmln = (XmlNode) xmlNode;
					if (xmln.Attributes != null && xmln.Attributes.Count > 0)
					{
						var xmlAttrVal = xmln.Attributes[0].Value;
						var xmlNodeName = xmln.Name;
						// test to see if Name exists
						if (!metaDataDictionary.ContainsKey(xmlNodeName))
						{
							metaDataDictionary.Add(xmlNodeName,
								new MetaDataTypeAndValue
								{
									MetadataType = xmlAttrVal,
									MetadataValue = xmln.InnerText
								});
						}
					}
				}
			}
			return metaDataDictionary;
		}

		/// <summary>
		/// Compounds the claim string for search.
		/// </summary>
		/// <param name="claimSchema">The claim schema.</param>
		/// <param name="claimProvider">The claim provider.</param>
		/// <param name="metaDataDictionary">The meta data dictionary.</param>
		/// <returns>System.String.</returns>
		public static string CompoundClaimStringForSearch(string claimSchema, string claimProvider,
			Dictionary<string, MetaDataTypeAndValue> metaDataDictionary)
		{
			const string permissionKey = "ariaClaims";
			var compoundClaimString = string.Empty;
			if (metaDataDictionary.ContainsKey(permissionKey))
			{
				var claims = !string.IsNullOrEmpty(metaDataDictionary[permissionKey].ToString())
					? metaDataDictionary[permissionKey].MetadataValue.Split(new[] {';'},
						StringSplitOptions
							.RemoveEmptyEntries)
					: new string[0];
				compoundClaimString = claims.Aggregate(compoundClaimString, (current, claim) => string.Concat(current, claimSchema, "|", claim, "|", claimProvider, Environment.NewLine));
			}

			return compoundClaimString;
		}

		private static string GetContainerDefinitionClaims(IAriaMetaDataRepository ariaMetaDataRepository, Guid containerId,
			Dictionary<string, MetaDataTypeAndValue> metadataDictionary)
		{
			var availableClaims = ariaMetaDataRepository.FetchAvailableClaimsByContainerAssetId(containerId);

			if (string.IsNullOrWhiteSpace(availableClaims))
			{
				throw new DatabaseItemNotFoundException(string.Format("Container {0} not found.", containerId));
			}

			if (metadataDictionary.ContainsKey(PermissionKey))
			{
				var permissionName = metadataDictionary[PermissionKey].MetadataValue;
				var doc = new XmlDocument();
				doc.LoadXml(availableClaims);
				// ReSharper disable once PossibleNullReferenceException
				var claimsElement = doc.DocumentElement.SelectSingleNode("List[Name='" + permissionName + "']/Claims");
				if (claimsElement != null)
				{
					return claimsElement.InnerText;
				}
			}

			return string.Empty;
		}

		private static string StreamToString(Stream stream)
		{
			stream.Position = 0;
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}
	}
}