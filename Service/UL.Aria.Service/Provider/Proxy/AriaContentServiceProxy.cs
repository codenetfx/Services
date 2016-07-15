using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

using UL.Enterprise.Foundation.Client;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider.Proxy
{
    /// <summary>
    ///     Class AriaContentServiceProxy
    /// </summary>
    public class AriaContentServiceProxy : ServiceAgentManagedProxy<IAriaContentService>, IAriaContentServiceProxy
    {
        private IAriaContentService _ariaContentService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AriaContentServiceProxy" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        protected AriaContentServiceProxy(ChannelFactory<IAriaContentService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
            //   ConfigureKnownTypes(serviceProxyFactory);
            var elements = serviceProxyFactory.Endpoint.Binding.CreateBindingElements();
            var transportBinding = elements.Find<HttpTransportBindingElement>();

            transportBinding.AuthenticationScheme = AuthenticationSchemes.Ntlm;

// ReSharper disable once PossibleNullReferenceException
            serviceProxyFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            serviceProxyFactory.Credentials.Windows.ClientCredential =
                CredentialCache.DefaultNetworkCredentials;

            serviceProxyFactory.Endpoint.Binding = new CustomBinding(elements);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AriaContentServiceProxy" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        public AriaContentServiceProxy(IProxyConfigurationSource serviceProxyFactory) :
            this(new WebChannelFactory<IAriaContentService>(serviceProxyFactory.AriaContentServiceEndPoint))
        {
        }

        /// <summary>
        ///     Deletes the container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        public bool DeleteContainer(string containerId)
        {
            bool deleted = false;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => deleted = _ariaContentService.DeleteContainer(containerId));
            }

            return deleted;
        }

        /// <summary>
        ///     Deletes the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        public bool DeleteAsset(string assetId)
        {
            bool deleted = false;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => deleted = _ariaContentService.DeleteAsset(assetId));
            }

            return deleted;
        }

        /// <summary>
        ///     Creates the asse link.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="assetId">The asset id.</param>
        public void CreateAssetLink(string parentId, string assetId)
        {
            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => _ariaContentService.CreateAssetLink(parentId, assetId));
            }
        }

        /// <summary>
        ///     Fetches the parent asset links.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        public IEnumerable<string> FetchParentAssetLinks(string parentId)
        {
            IEnumerable<string> parentAssetLinks = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => parentAssetLinks = _ariaContentService.FetchParentAssetLinks(parentId));
            }

            return parentAssetLinks;
        }

        /// <summary>
        ///     Fetches the asset links.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IEnumerable{System.String}.</returns>
        public IEnumerable<string> FetchAssetLinks(string assetId)
        {
            IEnumerable<string> assetLinks = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => assetLinks = _ariaContentService.FetchAssetLinks(assetId));
            }

            return assetLinks;
        }

        /// <summary>
        ///     Pings this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        public string Ping()
        {
            string pingResponse = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => pingResponse = _ariaContentService.Ping());
            }

            return pingResponse;
        }

        /// <summary>
        ///     Fetches all assets in a container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IEnumerable{AriaMetaDataItem}.</returns>
        public IEnumerable<AriaMetaDataItem> FetchAllAssets(string containerId)
        {
            IEnumerable<AriaMetaDataItem> metadataItems = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => metadataItems = _ariaContentService.FetchAllAssets(containerId));
            }

            return metadataItems;
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="createContainerRequest">The create container request.</param>
        /// <returns>System.String.</returns>
        public string CreateContainer(AriaCreateContainerRequestDto createContainerRequest)
        {
            string containerId = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => containerId = _ariaContentService.CreateContainer(createContainerRequest));
            }

            return containerId;
        }

        /// <summary>
        ///     Creates the asset.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public string CreateAsset(Stream data)
        {
            string assetId = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => assetId = _ariaContentService.CreateAsset(data));
            }

            return assetId;
        }

        /// <summary>
        ///     Deletes the asset link.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="assetId">The asset id.</param>
        public void DeleteAssetLink(string parentId, string assetId)
        {
            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                _ariaContentService.DeleteAssetLink(parentId, assetId);
            }
        }

        /// <summary>
        ///     Fetches the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>Stream.</returns>
        public Stream FetchAsset(string assetId)
        {
            return FetchAsset(assetId, "content");
        }

        /// <summary>
        ///     Updates the asset.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public string UpdateAsset(Stream data)
        {
            string assetId = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => assetId = _ariaContentService.UpdateAsset(data));
            }

            return assetId;
        }

        /// <summary>
        ///     Updates the container.
        /// </summary>
        /// <param name="updateContainerRequest">The update container request.</param>
        /// <returns>System.String.</returns>
        public string UpdateContainer(AriaUpdateContainerRequestDto updateContainerRequest)
        {
            string containerId = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => containerId = _ariaContentService.UpdateContainer(updateContainerRequest));
            }

            return containerId;
        }

        /// <summary>
        ///     Creates the container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String.</returns>
        public string CreateContainer(string container, string metaData)
        {
            var request = new AriaCreateContainerRequestDto
            {
                Container = container,
                MetaData = metaData,
            };
            return CreateContainer(request);
        }

        /// <summary>
        ///     Updates the container.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="metaData">The meta data.</param>
        /// <returns>System.String.</returns>
        public string UpdateContainer(string containerId, string metaData)
        {
            var updateContainerRequest = new AriaUpdateContainerRequestDto
            {
                ContainerId = containerId,
                MetaData = metaData,
            };

            return UpdateContainer(updateContainerRequest);
        }

        /// <summary>
        ///     Creates the asset.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public string CreateAsset(string containerId, string assetId, string streamType, Stream data)
        {
            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
// ReSharper disable once PossibleNullReferenceException
                WebOperationContext.Current.OutgoingRequest.Headers.Add("containerId", containerId);
                WebOperationContext.Current.OutgoingRequest.Headers.Add("assetId", assetId);
                WebOperationContext.Current.OutgoingRequest.Headers.Add("streamType", streamType);
                ExecuteAction(() => assetId = _ariaContentService.CreateAsset(data));
            }
            return assetId;
        }

        /// <summary>
        ///     Updates the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public string UpdateAsset(string assetId, string streamType, Stream data)
        {
            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
// ReSharper disable once PossibleNullReferenceException
                WebOperationContext.Current.OutgoingRequest.Headers.Add("assetId", assetId);
                WebOperationContext.Current.OutgoingRequest.Headers.Add("streamType", streamType);
                ExecuteAction(() => assetId = _ariaContentService.UpdateAsset(data));
            }
            return assetId;
        }

        /// <summary>
        ///     Fetches the asset.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="streamType">Type of the stream.</param>
        /// <returns>Stream.</returns>
        public Stream FetchAsset(string assetId, string streamType)
        {
            Stream stream = null;
            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
// ReSharper disable once PossibleNullReferenceException
                WebOperationContext.Current.OutgoingRequest.Headers.Add("streamType", streamType);
                ExecuteAction(() => stream = _ariaContentService.FetchAsset(assetId));
            }
            return stream;
        }

        /// <summary>
        ///     Fetches the asset metadata.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        public IDictionary<string, string> FetchAssetMetadata(string assetId)
        {
            Stream stream = null;

            try
            {
                stream = FetchAsset(assetId, "metadata");
                using (var streamReader = new StreamReader(stream))
                {
                    stream = null;
                    var metadataXml = streamReader.ReadToEnd();
                    return metadataXml.ToIDictionaryFromAriaSharepointXml();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        /// <summary>
        ///     Fetches the multiple parent asset links.
        /// </summary>
        /// <param name="parentIds">The parent ids.</param>
        /// <returns>IEnumerable{AriaMetaDataLinkDto}.</returns>
        public IEnumerable<AriaMetaDataLinkDto> FetchMultipleParentAssetLinks(string[] parentIds)
        {
            IEnumerable<AriaMetaDataLinkDto> assetLinks = null;

            _ariaContentService = ClientProxy;
// ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel) _ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => assetLinks = _ariaContentService.FetchMultipleParentAssetLinks(parentIds));
            }

            return assetLinks;
        }

        /// <summary>
        ///     Saves the assets.
        /// </summary>
        /// <param name="assets">The assets.</param>
        public void SaveAssets(IEnumerable<AriaAsset> assets)
        {
            _ariaContentService = ClientProxy;
            // ReSharper disable once SuspiciousTypeConversion.Global
            using (new OperationContextScope((IContextChannel)_ariaContentService))
            {
                AddCorrelationId();
                ExecuteAction(() => _ariaContentService.SaveAssets(assets));
            }
        }

        private void AddCorrelationId()
        {
// ReSharper disable once PossibleNullReferenceException
            WebOperationContext.Current.OutgoingRequest.Headers.Add("client-request-id",
                Trace.CorrelationManager.ActivityId.ToString());
        }
    }
}