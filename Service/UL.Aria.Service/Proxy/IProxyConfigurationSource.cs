using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.ServiceBus;

namespace UL.Aria.Service.Proxy
{
    /// <summary>
    /// Proxy configuration.
    /// </summary>
    public interface IProxyConfigurationSource
    {
        /// <summary>
        /// Gets or sets the message service binding.
        /// </summary>
        /// <value>
        /// The message service binding.
        /// </value>
        Binding MessageServiceBinding { get; }

        /// <summary>
        /// Gets the message service URI.
        /// </summary>
        /// <value>
        /// The message service URI.
        /// </value>
        Uri MessageServiceUri { get; }

        /// <summary>
        /// Gets the token provider.
        /// </summary>
        /// <value>
        /// The token provider.
        /// </value>
        TokenProvider TokenProvider { get; }

    }
}
