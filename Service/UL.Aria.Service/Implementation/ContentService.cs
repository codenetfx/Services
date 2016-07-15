using System.IO;
using System.ServiceModel;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class ContentService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class ContentService : IContentService
    {
        private readonly IAssetProvider _assetprovider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContentService" /> class.
        /// </summary>
        /// <param name="assetprovider">The assetprovider.</param>
        public ContentService(IAssetProvider assetprovider)
        {
            _assetprovider = assetprovider;
        }

        /// <summary>
        ///     Creates the content with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="contentStream">The content stream.</param>
        public void Create(string id, Stream contentStream)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(contentStream, "entityStream");
            _assetprovider.CreateContent(convertedId, contentStream);
        }

        /// <summary>
        ///     Updates the content specified by the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="contentStream">The content stream.</param>
        public void Update(string id, Stream contentStream)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(contentStream, "entityStream");
            _assetprovider.UpdateContent(convertedId, contentStream);
        }

        /// <summary>
        ///     Fetches the content specified by the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Stream.</returns>
        public Stream Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            return _assetprovider.FetchContent(convertedId);
        }
    }
}