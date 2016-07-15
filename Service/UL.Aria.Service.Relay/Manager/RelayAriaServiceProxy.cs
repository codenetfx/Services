using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayAriaServiceProxy : ServiceAgentManagedProxy<IAriaService>, IAriaService
    {
        private readonly IPrincipalResolver _principalResolver;
        private WebChannelFactory<IAriaService> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public RelayAriaServiceProxy(IProxyConfigurationSource configurationSource, IPrincipalResolver principalResolver) :
            this(
            new WebChannelFactory<IAriaService>(new WebHttpBinding(), configurationSource.AriaService))
        {
            _principalResolver = principalResolver;
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private RelayAriaServiceProxy(WebChannelFactory<IAriaService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
            _factory = serviceProxyFactory;
        }

        /// <summary>
        /// Fetches all assets in a container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// SearchResultSetDto.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public SearchResultSetDto FetchAllAssets(string id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Fetches the type of all assets of entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public SearchResultSetDto FetchAllDocuments(string id)
        {
            SearchResultSetDto searchResultSetDto = null;
            IAriaService ariaService = ClientProxy;
            using (new OperationContextScope((IContextChannel)ariaService))
            {
                searchResultSetDto = ariaService.FetchAllDocuments(id);
                return searchResultSetDto;
            }
        }

        /// <summary>
        /// Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public SearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
            SearchResultSetDto searchResultSetDto = null;
            IAriaService ariaService = ClientProxy;
            searchCriteria.UserId = _principalResolver.UserId;
            using (new OperationContextScope((IContextChannel)ariaService))
            {
                searchResultSetDto = ariaService.Search(searchCriteria);
                return searchResultSetDto;
            }
        }

        /// <summary>
        /// Publishes the fetchmetadata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>
        /// The metadata stream.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public IDictionary<string, string> PublishFetchMetadata(string entityType, string id, string assetType, string assetId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Publishes the createmetdata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <returns>
        /// The saved content id.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string PublishCreateMetadata(string entityType, string id, string assetType, IDictionary<string, string> metadataStream)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Publishes the create product.
        /// </summary>
        /// <param name="newContainerId">The new container id.</param>
        /// <param name="newAssetId">The new asset id.</param>
        /// <param name="product">The product.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string PublishCreateProduct(string newContainerId, string newAssetId, ProductDto product)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Publishes the updatemetdata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void PublishUpdateMetadata(string entityType, string id, string assetType, string assetId, IDictionary<string, string> metadataStream)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Deletes the content.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void PublishDeleteContent(string entityType, string entityId, string assetType, string assetId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Fetches all by company id.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public SearchResultSetDto FetchAllContainers(SearchCriteriaDto searchCriteria)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the available, assignable claims for this container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public IList<Claim> GetAvailableUserClaims(string id)
        {
            throw new NotSupportedException();
        }

		/// <summary>
		/// Creates the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		/// <exception cref="System.NotSupportedException"></exception>
        public void CreateAssetLink(AssetLinkDto assetLink)
        {
            throw new NotSupportedException();
        }

		/// <summary>
		/// Deletes the asset link.
		/// </summary>
		/// <param name="assetLink">The asset link.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public void DeleteAssetLink(AssetLinkDto assetLink)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Fetches the asset links.
        /// </summary>
        /// <param name="parentAssetType">Type of the parent asset.</param>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>
        /// IList{System.String}.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public IList<string> FetchAssetLinks(string parentAssetType, string parentAssetId, string assetType, string assetId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Fetches the parent asset links.
        /// </summary>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <returns>
        /// IList{System.String}.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public IList<string> FetchParentAssetLinks(string parentAssetId)
        {
            throw new NotSupportedException();
        }
    }
}