using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.ServiceBus;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.InboundOrderProcessing.Service
{
    /// <summary>
    ///     Configuration Options for service proxies.
    /// </summary>
    public class ProxyConfigurationSource : IProxyConfigurationSource
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyConfigurationSource"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ProxyConfigurationSource()
        {
            var serviceConfiguration = (AzureWcfProcessConfigurationSection)ConfigurationManager.GetSection("AzureWcfProcessConfiguration");
            var azureWcfProcessConfiguration = serviceConfiguration == null || serviceConfiguration.Services.Count == 0 ? null : serviceConfiguration.Services[0];
            if (azureWcfProcessConfiguration != null && azureWcfProcessConfiguration.ServiceBusEnabled)
            {
                TokenProvider =
                    TokenProvider.CreateSharedSecretTokenProvider(
                        azureWcfProcessConfiguration.ServiceBusIssuer,
                        azureWcfProcessConfiguration.ServiceBusKey);
                CustomerPartyServiceBinding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.Transport,
                    RelayClientAuthenticationType.RelayAccessToken);
                CustomerPartyServiceUri =
                    ServiceBusEnvironment.CreateServiceUri(
                        azureWcfProcessConfiguration.ServiceBusScheme,
                        azureWcfProcessConfiguration.ServiceBusNamespace, "CustomerParty");
            }
            else
            {
                CustomerPartyServiceBinding = new WebHttpBinding();
                var uriString = ConfigurationManager.AppSettings["UL.CustomerPartyServiceUri"];
                CustomerPartyServiceUri = new Uri(uriString);
                TokenProvider = null;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [store original XML].
        /// </summary>
        /// <value><c>true</c> if [store original XML]; otherwise, <c>false</c>.</value>
        public bool StoreOriginalXml
        {
            get
            {
                string appSetting = ConfigurationManager.AppSettings["storeOriginalXml"];
                return string.IsNullOrWhiteSpace(appSetting) || Convert.ToBoolean(appSetting);
            }
        }

        /// <summary>
        ///     Gets the order message service.
        /// </summary>
        /// <value>
        ///     The order message service.
        /// </value>
        public Uri NewOrderService
        {
            get
            {
                string appSetting = ConfigurationManager.AppSettings["newOrderServiceUrl"];
                return !string.IsNullOrWhiteSpace(appSetting) ? new Uri(appSetting) : null;
            }
        }

        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>
        /// The token provider.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public TokenProvider TokenProvider { get; private set; }

        /// <summary>
        /// Gets or sets the customer party service URI.
        /// </summary>
        /// <value>
        /// The customer party service URI.
        /// </value>
        public Uri CustomerPartyServiceUri { get; set; }

        /// <summary>
        /// Gets or sets the customer party service binding.
        /// </summary>
        /// <value>
        /// The customer party service binding.
        /// </value>
        public Binding CustomerPartyServiceBinding { get; set; }
    }
}