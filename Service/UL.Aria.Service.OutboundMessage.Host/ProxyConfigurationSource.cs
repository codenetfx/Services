using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.ServiceBus;
using UL.Aria.Common;
using UL.Aria.Service.Proxy;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.OutboundMessage.Host
{
    /// <summary>
    /// Implementation of proxy configuration.
    /// </summary>
    public class ProxyConfigurationSource : IProxyConfigurationSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyConfigurationSource"/> class.
        /// </summary>
        public ProxyConfigurationSource()
        {
			var serviceConfiguration = (AzureWcfProcessConfigurationSection)ConfigurationManager.GetSection("AzureWcfProcessConfiguration");
			var azureWcfProcessConfiguration = serviceConfiguration == null || serviceConfiguration.Services.Count == 0 ? null : serviceConfiguration.Services[0];
			if (azureWcfProcessConfiguration != null && azureWcfProcessConfiguration.ServiceBusEnabled)
			{
                MessageServiceBinding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.Transport, RelayClientAuthenticationType.RelayAccessToken);
				MessageServiceUri = ServiceBusEnvironment.CreateServiceUri(azureWcfProcessConfiguration.ServiceBusScheme, azureWcfProcessConfiguration.ServiceBusNamespace, "Message");
				TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(azureWcfProcessConfiguration.ServiceBusIssuer, azureWcfProcessConfiguration.ServiceBusKey);
            }
            else
            {
                MessageServiceBinding = new WebHttpBinding();
                var uriString = ConfigurationManager.AppSettings["UL.RelayServiceUri"];
                MessageServiceUri = new Uri(uriString);
                TokenProvider = null;
            }
        }

        /// <summary>
        /// Gets the message service URI.
        /// </summary>
        /// <value>
        /// The message service URI.
        /// </value>
        public Uri MessageServiceUri { get; private set; }

        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>
        /// The token provider.
        /// </value>
        public TokenProvider TokenProvider { get; private set; }

        /// <summary>
        /// Gets or sets the message service binding.
        /// </summary>
        /// <value>
        /// The message service binding.
        /// </value>
        public Binding MessageServiceBinding { get; private set; }
    }
}
