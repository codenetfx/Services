using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.Provider.Proxy
{
    /// <summary>
    ///     Interface IAriaContentService
    /// </summary>
    [ServiceContract]
    public interface IAriaContentServiceProxy : IAriaContentService
    {
        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String.</returns>
        string CreateContainer(string container, string metaData);

        /// <summary>
        ///     Creates the asset.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        string CreateAsset(string containerId, string assetId, string streamType, Stream data);

        /// <summary>
        ///     Updates the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        string UpdateAsset(string assetId, string streamType, Stream data);

        /// <summary>
        ///     Fetches the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <returns>Stream.</returns>
        Stream FetchAsset(string assetId, string streamType);

        /// <summary>
        ///     Updates the container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String.</returns>
        string UpdateContainer(string containerId, string metaData);

        /// <summary>
        ///     Fetches the asset metadata.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        IDictionary<string, string> FetchAssetMetadata(string assetId);
    }
}