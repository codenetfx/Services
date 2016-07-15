using System;

namespace UL.Aria.Service.Provider.Proxy
{
    /// <summary>
    ///     An interface to allow for the injection of a proxies EndPoint i.e. http://localhost:807/ProfileService
    /// </summary>
    public interface IProxyConfigurationSource
    {
        /// <summary>
        ///     Gets or sets the aria task service end point.
        /// </summary>
        /// <value>The aria task service end point.</value>
        Uri AriaTaskServiceEndPoint { get; set; }

        /// <summary>
        ///     Gets or sets the aria content service end point.
        /// </summary>
        /// <value>The aria content service end point.</value>
        Uri AriaContentServiceEndPoint { get; set; }
    }
}