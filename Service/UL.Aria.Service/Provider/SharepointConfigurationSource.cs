using System;
using System.Configuration;
using System.Net;
using UL.Aria.Common;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Configuration Options for service proxies.
    /// </summary>
    public class SharepointConfigurationSource : ISharepointConfigurationSource
    {
        private string _host = Environment.MachineName;

        /// <summary>
        ///     Gets the task service URL.
        /// </summary>
        /// <value>The task service URL.</value>
        public Uri TaskService
        {
            get
            {
                var appSetting = ConfigurationManager.AppSettings["UL.Sharepoint.TaskService"];
                return string.IsNullOrWhiteSpace(appSetting)
                           ? new Uri(string.Format("http://{0}/_vti_bin/aria/taskservice.svc", DefaultHost))
                           : new Uri(appSetting);
            }
        }

        /// <summary>
        ///     Gets the content service.
        /// </summary>
        /// <value>The content service.</value>
        public Uri ContentService
        {
            get
            {
                var contentService = ConfigurationManager.AppSettings["UL.Sharepoint.ContentService"];
                return string.IsNullOrWhiteSpace(contentService)
                           ? new Uri(string.Format("http://{0}/_vti_bin/aria/contentservice.svc",
                                                   DefaultHost))
                           : new Uri(contentService);
            }
        }

        /// <summary>
        ///     Gets the sharepoint search service uri.
        /// </summary>
        /// <value>
        ///     The sharepoint search service urie.
        /// </value>
        /// <remarks>
        /// The SharePoint search service is used on a different port from the custom Aria SharePoint REST Services.
        /// The SharePoint Web Application has been extended to support a 2nd 'zone'.
        /// Gets the uri from the web.config AppSettings.  Defaults to use the local computername for the web application name and use port 801.
        /// Example:   http://computername:803/_api_search/postquery
        /// </remarks>
        public Uri SearchService
        {
            get
            {
                var appSetting = ConfigurationManager.AppSettings["UL.Sharepoint.SearchService"];
                return string.IsNullOrWhiteSpace(appSetting)
                           ? new Uri(string.Format("http://{0}:803/_api/search/postquery", DefaultHost))
                           : new Uri(appSetting);
            }
        }

        /// <summary>
        ///     Gets the network credentials.
        /// </summary>
        /// <value>
        ///     The network credentials.
        /// </value>
        public NetworkCredential Credentials
        {
            get
            {
                var userName = ConfigurationManager.AppSettings["UL.Sharepoint.UserName"];

                if (!string.IsNullOrEmpty(userName))
                {
                    var securePassword = ConfigurationManager.AppSettings["UL.Sharepoint.SecurePassword"];
                    if (!string.IsNullOrEmpty(securePassword))
                    {
                        return new NetworkCredential(userName, securePassword);
                    }

                    var password = ConfigurationManager.AppSettings["UL.Sharepoint.Password"];
                    if (!string.IsNullOrEmpty(password))
                    {
                        return new NetworkCredential(userName, password);
                    }
                }

                return CredentialCache.DefaultNetworkCredentials;
            }
        }

		/// <summary>
		/// Gets or sets the time-out value in milliseconds
		/// </summary>
		/// <returns>
		/// The number of milliseconds to wait before the request times out. The default value is 30,000 milliseconds (30 seconds).
		///   </returns>
	    public int Timeout
	    {
		    get { return ConfigurationManager.AppSettings.GetValue("UL.Sharepoint.Timeout", 30*1000); }
	    }

        internal string DefaultHost
        {
            get { return _host; }
        }
    }
}