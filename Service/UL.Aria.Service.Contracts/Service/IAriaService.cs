using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Aria service contract provides a mechanism for managing entity persistence and search.
    /// </summary>
    [ServiceContract]
    public interface IAriaService
    {
        /// <summary>
        ///     Fetches all assets in a container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>SearchResultSetDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Metadata", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        SearchResultSetDto FetchAllAssets(string id);

        /// <summary>
        /// Fetches the type of all assets of entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}/Document/Metadata", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        SearchResultSetDto FetchAllDocuments(string id);

        /// <summary>
        ///     Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        SearchResultSetDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        ///     Publishes the fetchmetadata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>The metadata stream.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityType}/{id}/{assetType}/{assetId}/Metadata", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IDictionary<string, string> PublishFetchMetadata(string entityType, string id, string assetType, string assetId);

        /// <summary>
        ///     Publishes the createmetdata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <returns>The saved content id.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityType}/{id}/{assetType}/Metadata", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string PublishCreateMetadata(string entityType, string id, string assetType,
            IDictionary<string, string> metadataStream);

        /// <summary>
        ///     Publishes the create product.
        /// </summary>
        /// <param name="newContainerId">The new container id.</param>
        /// <param name="newAssetId">The new asset id.</param>
        /// <param name="product">The product.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Product/{newContainerId}/Asset/{newAssetId}", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string PublishCreateProduct(string newContainerId, string newAssetId, ProductDto product);

        /// <summary>
        ///     Publishes the updatemetdata.
        /// </summary>
        /// <param name="entityType">The type of the entity the metadata is for.</param>
        /// <param name="id">The id.</param>
        /// <param name="assetType">The type of the entity asset the metadata is for.</param>
        /// <param name="assetId">The asset id.</param>
        /// <param name="metadataStream">The metadata stream.</param>
        /// <returns>The saved content id.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityType}/{id}/{assetType}/{assetId}/Metadata", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void PublishUpdateMetadata(string entityType, string id, string assetType, string assetId,
            IDictionary<string, string> metadataStream);

        /// <summary>
        ///     Deletes the content.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityType}/{entityId}/{assetType}/{assetId}", Method = "DELETE",
            RequestFormat = WebMessageFormat.Json)]
        void PublishDeleteContent(string entityType, string entityId, string assetType, string assetId);

        /// <summary>
        ///     Fetches all by company id.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/FetchAll", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        SearchResultSetDto FetchAllContainers(SearchCriteriaDto searchCriteria);


        /// <summary>
        ///     Gets the available, assignable claims for this container.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/{id}/claims", Method = "GET", RequestFormat = WebMessageFormat.Json)]
        IList<Claim> GetAvailableUserClaims(string id);

        /// <summary>
        ///     Creates the asset link.
        /// </summary>
        [WebInvoke(UriTemplate = "/AssetLink",
            Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void CreateAssetLink(AssetLinkDto assetLink);


        /// <summary>
        ///     Deletes the asset link.
        /// </summary>
        [WebInvoke(UriTemplate = "/AssetLink",
            Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
		void DeleteAssetLink(AssetLinkDto assetLink);

        /// <summary>
        ///     Fetches the asset links.
        /// </summary>
        /// <param name="parentAssetType">Type of the parent asset.</param>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IList{System.String}.</returns>
        [WebInvoke(UriTemplate = "/AssetLink/{parentAssetType}/{parentAssetId}/{assetType}/{assetId}",
            Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<string> FetchAssetLinks(string parentAssetType, string parentAssetId, string assetType,
            string assetId);

        /// <summary>
        ///     Fetches the parent asset links.
        /// </summary>
        /// <param name="parentAssetId">The parent asset id.</param>
        /// <returns>IList{System.String}.</returns>
        [WebInvoke(UriTemplate = "/AssetLink/{parentAssetId}",
            Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<string> FetchParentAssetLinks(string parentAssetId);
    }
}