using System;
using System.ServiceModel.Channels;

using Microsoft.ServiceBus;

namespace UL.Aria.Service.Proxy
{
	/// <summary>
	/// Interface IOutboundDocumentProxyConfigurationSource
	/// </summary>
	public interface IOutboundDocumentProxyConfigurationSource
	{
		/// <summary>
		/// Gets the token provider.
		/// </summary>
		/// <value>
		/// The token provider.
		/// </value>
		TokenProvider TokenProvider { get; }

		/// <summary>
		/// Gets or sets the outbound document service URI.
		/// </summary>
		/// <value>The outbound document service URI.</value>
		Uri OutboundDocumentServiceUri { get; set; }

		/// <summary>
		/// Gets or sets the outbound document service binding.
		/// </summary>
		/// <value>The outbound document service binding.</value>
		Binding OutboundDocumentServiceBinding { get; set; }
	}
}