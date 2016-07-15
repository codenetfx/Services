using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IAriaContentService
    /// </summary>
    [ServiceContract]
    public interface IAriaContentService
    {
        /// <summary>
        ///     Fetches all assets.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IEnumerable{AriaMetaDataItem}.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Containers/{containerId}/Assets/",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        IEnumerable<AriaMetaDataItem> FetchAllAssets(string containerId);

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Containers", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string CreateContainer(AriaCreateContainerRequestDto request);

        /// <summary>
        ///     Updates the container.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Containers", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string UpdateContainer(AriaUpdateContainerRequestDto request);

        /// <summary>
        ///     Deletes the container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Containers/{containerId}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool DeleteContainer(string containerId);

        /// <summary>
        ///     Creates the asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Assets", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string CreateAsset(Stream asset);

        /// <summary>
        ///     Updates the asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Assets", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string UpdateAsset(Stream asset);

        /// <summary>
        ///     Deletes the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Assets/{assetId}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool DeleteAsset(string assetId);

        /// <summary>
        ///     Fetches the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>Stream.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Assets/{assetId}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream FetchAsset(string assetId);

        /// <summary>
        ///     Creates the asse link.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="assetId">The asset id.</param>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AssetLinks", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void CreateAssetLink(string parentId, string assetId);

        /// <summary>
        ///     Deletes the asset link.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="assetId">The asset id.</param>
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "AssetLinks/{parentId}/{assetId}",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteAssetLink(string parentId, string assetId);

        /// <summary>
        ///     Fetches the parent asset links.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AssetLinks/Parent/{parentId}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> FetchParentAssetLinks(string parentId);

        /// <summary>
        ///     Fetches the asset links.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "AssetLinks/Asset/{assetId}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> FetchAssetLinks(string assetId);

        /// <summary>
        ///     Pings this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string Ping();

        /// <summary>
        ///     Fetches the multiple parent asset links.
        /// </summary>
        /// <param name="parentIds">The parent ids.</param>
        /// <returns>IEnumerable{AriaMetaDataLinkDto}.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AssetLinks/Parents", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<AriaMetaDataLinkDto> FetchMultipleParentAssetLinks(string[] parentIds);

        /// <summary>
        ///     Saves the assets.
        /// </summary>
        /// <param name="assets">The assets.</param>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SaveAssets", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void SaveAssets(IEnumerable<AriaAsset> assets);
    }
}