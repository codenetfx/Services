using System;
using System.Collections.Generic;
using System.ServiceModel;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     The aria service implementation provides a mechanism for managing entity persistence and search.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class AriaService : IAriaService
    {
        private readonly IAssetProvider _assetprovider;
        private readonly IContainerManager _containerManager;
	    private readonly IMapperRegistry _mapperRegistry;
        private readonly ISearchProvider _searchProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="AriaService" /> class.
		/// </summary>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="assetprovider">The contentprovider.</param>
		/// <param name="searchProvider">The search provider.</param>
		/// <param name="containerManager">The container manager.</param>
        public AriaService(IMapperRegistry mapperRegistry, IAssetProvider assetprovider, ISearchProvider searchProvider,
			IContainerManager containerManager)
        {
            _mapperRegistry = mapperRegistry;
            _assetprovider = assetprovider;
            _searchProvider = searchProvider;
            _containerManager = containerManager;
        }

        /// <summary>
        ///     Fetches all assets in a container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SearchResultSetDto.</returns>
        public SearchResultSetDto FetchAllAssets(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            var searchResultSet = _assetprovider.FetchAllAssets(convertedId);
            return _mapperRegistry.Map<SearchResultSetDto>(searchResultSet);
        }

        /// <summary>
        /// Fetches the type of all assets of entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public SearchResultSetDto FetchAllDocuments(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
           
            var searchResultSet = _assetprovider.FetchAllDocuments(convertedId);
            return _mapperRegistry.Map<SearchResultSetDto>(searchResultSet);
        }

        /// <summary>
        ///     Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>The search result set.</returns>
        public SearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
            Guard.IsNotNull(searchCriteria, "searchCriteria");

            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
            var result = _searchProvider.Search(criteria);

            var searchResultSetDto = _mapperRegistry.Map<SearchResultSetDto>(result);
            return searchResultSetDto;

            //Mimicks the behavior of the existing search service with the exception
            //that the search will be for all types (i.e. not just containers).
            //At this layer, the search will just map the dto -> search criteria and invoke provider
            //Create a new search provider implemation which will support type agnostic searches, validate search criteria
            //using the existing search provider as a base.
            //Search repo will provide build the SP-specific search query based on teh search specification.  
        }

        /// <summary>
        ///     Publishes the createmetadata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <returns>
        ///     The saved content id.
        /// </returns>
        public string PublishCreateMetadata(string entityType, string entityId, string assetType,
            IDictionary<string, string> metadataStream)
        {
            Guard.IsNotNullOrEmptyTrimmed(entityType, "entityType");
            Guard.IsNotNullOrEmptyTrimmed(assetType, "assetType");
            Guard.IsNotNull(metadataStream, "entityStream");

            return _assetprovider.Create(entityId.ToGuid(), metadataStream, Guid.NewGuid());
        }

        /// <summary>
        ///     Publishes the create product.
        /// </summary>
        /// <param name="newContainerId">The new container id.</param>
        /// <param name="newAssetId">The new asset id.</param>
        /// <param name="product">The product.</param>
        /// <returns>System.String.</returns>
        public string PublishCreateProduct(string newContainerId, string newAssetId, ProductDto product)
        {
            var productBo = _mapperRegistry.Map<Product>(product);
            return _assetprovider.Create(newContainerId.ToGuid(), newAssetId.ToGuid(), productBo).ToString();
        }

        /// <summary>
        ///     Publishes the updatemetadata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        public void PublishUpdateMetadata(string entityType, string entityId, string assetType, string assetId,
            IDictionary<string, string> metadataStream)
        {
            Guard.IsNotNullOrEmptyTrimmed(entityType, "entityType");
            Guard.IsNotNullOrEmptyTrimmed(assetType, "assetType");
            Guard.IsNotNull(metadataStream, "entityStream");

            if (string.IsNullOrEmpty(assetId))
                assetId = Guid.Empty.ToString();

            _assetprovider.Update(assetId.ToGuid(), metadataStream);
        }

        /// <summary>
        ///     Publishes the fetchmetadata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>
        ///     The metadata stream.
        /// </returns>
        public IDictionary<string, string> PublishFetchMetadata(string entityType, string entityId, string assetType,
            string assetId)
        {
            Guard.IsNotNullOrEmptyTrimmed(entityType, "entityType");
            Guard.IsNotNullOrEmptyTrimmed(assetType, "assetType");

            return _assetprovider.Fetch(assetId.ToGuid());
        }

        /// <summary>
        ///     Deletes the content.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        public void PublishDeleteContent(string entityType, string entityId, string assetType, string assetId)
        {
            Guard.IsNotNullOrEmptyTrimmed(entityType, "entityType");
            Guard.IsNotNullOrEmptyTrimmed(assetType, "assetType");

            _assetprovider.DeleteContent(assetId.ToGuid());
        }

        /// <summary>
        ///     Gets the available, assignable claims for this container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public IList<System.Security.Claims.Claim> GetAvailableUserClaims(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            return _containerManager.GetAvailableUserClaims(convertedId);
        }

        /// <summary>
        ///     Fetches all by company id.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public SearchResultSetDto FetchAllContainers(SearchCriteriaDto searchCriteria)
        {
            Guard.IsNotNull(searchCriteria, "searchCriteria");
            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
            var result = _containerManager.GetByCompanyId(criteria);

            var searchResultSetDto = _mapperRegistry.Map<SearchResultSetDto>(result);
            return searchResultSetDto;
        }

		/// <summary>
		/// Creates the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
        public void CreateAssetLink(AssetLinkDto assetLink)
        {
			Guard.IsNotNull(assetLink, "assetLink");

			var assetLinkBo = _mapperRegistry.Map<AssetLink>(assetLink);

			_assetprovider.CreateAssetLink(assetLinkBo);
        }

		/// <summary>
		/// Deletes the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		public void DeleteAssetLink(AssetLinkDto assetLink)
        {
			Guard.IsNotNull(assetLink, "assetLink");

			var assetLinkBo = _mapperRegistry.Map<AssetLink>(assetLink);

			_assetprovider.DeleteAssetLink(assetLinkBo);
        }

        /// <summary>
        ///     Fetches the asset links.
        /// </summary>
        /// <param name="parentAssetType">Type of the parent asset.</param>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IList{System.String}.</returns>
        public IList<string> FetchAssetLinks(string parentAssetType, string parentAssetId, string assetType,
            string assetId)
        {
            Guard.IsNotNullOrEmpty(parentAssetId, "parentAssetType");
            Guard.IsNotNullOrEmpty(parentAssetId, "parentAssetId");
            var parentAssetIdGuid = parentAssetId.ToGuid();
            Guard.IsNotEmptyGuid(parentAssetIdGuid, "parentAssetId");
            Guard.IsNotNullOrEmpty(parentAssetId, "assetType");
            Guard.IsNotNullOrEmpty(assetId, "assetId");
            var assetIdGuid = assetId.ToGuid();
            Guard.IsNotEmptyGuid(assetIdGuid, "assetId");

            return _assetprovider.FetchAssetLinks(assetIdGuid);
        }

        /// <summary>
        ///     Fetches the parent asset links.
        /// </summary>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <returns>IList{System.String}.</returns>
        public IList<string> FetchParentAssetLinks(string parentAssetId)
        {
            Guard.IsNotNullOrEmpty(parentAssetId, "parentAssetId");
            var parentAssetIdGuid = parentAssetId.ToGuid();
            Guard.IsNotEmptyGuid(parentAssetIdGuid, "parentAssetId");

            return _assetprovider.FetchParentAssetLinks(parentAssetIdGuid);
        }
    }
}