using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.ServiceBus;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    /// Proxy configuration definition.
    /// </summary>
    public class CertificationRequestServiceConfigurationSource : ICertificationRequestServiceConfigurationSource
    { /// <summary>
    /// Initializes a new instance of the <see cref="CertificationRequestServiceConfigurationSource"/> class.
    /// </summary>

        [ExcludeFromCodeCoverage]
        public CertificationRequestServiceConfigurationSource()
    {
        var serviceConfiguration = (AzureWcfProcessConfigurationSection)ConfigurationManager.GetSection("AzureWcfProcessConfiguration");
        var azureWcfProcessConfiguration = serviceConfiguration == null || serviceConfiguration.Services.Count == 0 ? null : serviceConfiguration.Services[0];
        if (azureWcfProcessConfiguration != null && azureWcfProcessConfiguration.ServiceBusEnabled)
        {
            TokenProvider =
                TokenProvider.CreateSharedSecretTokenProvider(
                    azureWcfProcessConfiguration.ServiceBusIssuer,
                    azureWcfProcessConfiguration.ServiceBusKey);
            CertificationRequestServiceBinding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.Transport,
                RelayClientAuthenticationType.RelayAccessToken);
            CertificationRequestServiceUri =
                ServiceBusEnvironment.CreateServiceUri(
                    azureWcfProcessConfiguration.ServiceBusScheme,
                    azureWcfProcessConfiguration.ServiceBusNamespace, "CertificationRequest");
        }
        else
        {
            CertificationRequestServiceBinding = new WebHttpBinding();
            var uriString = ConfigurationManager.AppSettings["UL.CertificationRequestServiceUri"];
            CertificationRequestServiceUri = new Uri(uriString);
            TokenProvider = null;
        }
    }
        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>
        /// The token provider.
        /// </value>
        public TokenProvider TokenProvider { get; private set; }

        /// <summary>
        /// Gets or sets the certification request service URI.
        /// </summary>
        /// <value>
        /// The certification request service URI.
        /// </value>
        public Uri CertificationRequestServiceUri { get; set; }

        /// <summary>
        /// Gets or sets the certification request service binding.
        /// </summary>
        /// <value>
        /// The certification request service binding.
        /// </value>
        public Binding CertificationRequestServiceBinding { get; set; }
    }
}