using System;
using UL.Aria.Service.Proxy;

namespace UL.Aria.Service.InboundOrderProcessing.Service
{
    /// <summary>
    /// Configuration Options for service proxies.
    /// </summary>
    public interface IProxyConfigurationSource : ICustomerPartyProxyConfigurationSource
    {
        /// <summary>
        /// Gets a value indicating whether [store original XML].
        /// </summary>
        /// <value><c>true</c> if [store original XML]; otherwise, <c>false</c>.</value>
        bool StoreOriginalXml { get; }

        /// <summary>
        /// Gets the order message service.
        /// </summary>
        /// <value>
        /// The order message service.
        /// </value>
        Uri NewOrderService { get; }


    }
}