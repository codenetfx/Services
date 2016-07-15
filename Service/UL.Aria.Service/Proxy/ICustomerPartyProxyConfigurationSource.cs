using System;
using System.ServiceModel.Channels;
using Microsoft.ServiceBus;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    /// Proxy configuration.
    /// </summary>
    public interface ICustomerPartyProxyConfigurationSource
    {

        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>
        /// The token provider.
        /// </value>
        TokenProvider TokenProvider { get; }

        /// <summary>
        /// Gets or sets the customer party service URI.
        /// </summary>
        /// <value>
        /// The customer party service URI.
        /// </value>
        Uri CustomerPartyServiceUri { get; set; }

        /// <summary>
        /// Gets or sets the customer party service binding.
        /// </summary>
        /// <value>
        /// The customer party service binding.
        /// </value>
        Binding CustomerPartyServiceBinding { get; set; }
    }
}