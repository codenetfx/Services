using System;

namespace UL.Aria.Service.Provider.Proxy
{
    /// <summary>
    ///     Will provide URIs for all service enpoints for proxy classes use.
    /// </summary>
    public class ProxyConfigurationSource : IProxyConfigurationSource
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProxyConfigurationSource" /> class.
        /// </summary>
        /// <param name="sharepointConfigurationSource">The sharepoint configuration source.</param>
        public ProxyConfigurationSource(ISharepointConfigurationSource sharepointConfigurationSource)
        {
            AriaTaskServiceEndPoint = sharepointConfigurationSource.TaskService;
            AriaContentServiceEndPoint = sharepointConfigurationSource.ContentService;
        }

        /// <summary>
        ///     Gets or sets the task service end point.
        /// </summary>
        /// <value>The task service end point.</value>
        public Uri AriaTaskServiceEndPoint { get; set; }

        /// <summary>
        ///     Gets or sets the aria content service end point.
        /// </summary>
        /// <value>The aria content service end point.</value>
        public Uri AriaContentServiceEndPoint { get; set; }
    }
}