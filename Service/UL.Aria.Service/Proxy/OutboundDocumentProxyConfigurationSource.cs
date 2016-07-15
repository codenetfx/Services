using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Microsoft.ServiceBus;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Proxy
{
	/// <summary>
	/// Class OutboundDocumentProxyConfigurationSource.
	/// </summary>
	public class OutboundDocumentProxyConfigurationSource : IOutboundDocumentProxyConfigurationSource
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OutboundDocumentProxyConfigurationSource"/> class.
		/// </summary>
		public OutboundDocumentProxyConfigurationSource()
		{
			var serviceConfiguration =
				(AzureWcfProcessConfigurationSection) ConfigurationManager.GetSection("AzureWcfProcessConfiguration");
			var azureWcfProcessConfiguration = serviceConfiguration == null || serviceConfiguration.Services.Count == 0
				? null
				: serviceConfiguration.Services[0];
			if (azureWcfProcessConfiguration != null && azureWcfProcessConfiguration.ServiceBusEnabled)
			{
				TokenProvider =
					TokenProvider.CreateSharedSecretTokenProvider(
						azureWcfProcessConfiguration.ServiceBusIssuer,
						azureWcfProcessConfiguration.ServiceBusKey);
				OutboundDocumentServiceBinding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.Transport,
					RelayClientAuthenticationType.RelayAccessToken);
				OutboundDocumentServiceUri =
					ServiceBusEnvironment.CreateServiceUri(
						azureWcfProcessConfiguration.ServiceBusScheme,
						azureWcfProcessConfiguration.ServiceBusNamespace, "OutboundDocument");
			}
			else
			{
				OutboundDocumentServiceBinding = new WebHttpBinding();
				var uriString = ConfigurationManager.AppSettings.GetValue("UL.OutboundDocumentServiceUri", string.Empty);
			    if (!string.IsNullOrWhiteSpace(uriString))
			    {
			        OutboundDocumentServiceUri = new Uri(uriString);
			    }
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
		/// Gets or sets the outbound document service URI.
		/// </summary>
		/// <value>The outbound document service URI.</value>
		public Uri OutboundDocumentServiceUri { get; set; }

		/// <summary>
		/// Gets or sets the outbound document service binding.
		/// </summary>
		/// <value>The outbound document service binding.</value>
		public Binding OutboundDocumentServiceBinding { get; set; }
	}
}