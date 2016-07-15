using System;
using System.Collections.Generic;
using System.Configuration;

using UL.Enterprise.Foundation;
using UL.Aria.Web.Common.Configuration;
using UL.Aria.Web.Common.Models.Container;
using UL.Aria.Web.Common.Models.Shared;

namespace UL.Aria.Service.ProductProcessing.Host
{
    internal class ServiceConfiguration : IPortalConfiguration
    {
        public ServiceConfiguration()
        {
            var settings = ConfigurationManager.AppSettings;
            
			var serviceRoot = settings.GetValue("UL.ServiceRootUri", "http://ariasvc:802/");
            ServiceRootUri = new Uri(serviceRoot);

            DocumentTypes = new DocumentType[] {};
            Industries = new Dictionary<string, string>();
            UlApps = new List<MenuItem>(0);
			DocumentContentServiceProxy = new DocumentContentServiceProxySection();

            // Default to the 'Basic' task type ID.
            FreeformTaskTypeId = settings.GetValue(ConfigKeys.FreeformTaskTypeId, Guid.Parse("BEE714E0-FF7E-41B7-9B25-4D628F4FEF45"));
        }

		/// <summary>
		/// Gets or sets the departments.
		/// </summary>
		/// <value>
		/// The departments.
		/// </value>
		public Dictionary<string, string> Departments { get; set; }

		/// <summary>
		/// Gets or sets the MIME types.
		/// </summary>
		/// <value>
		/// The MIME types.
		/// </value>
		public Dictionary<string, string> MimeTypes { get; set; }

        /// <summary>
        ///     Gets the order service XSLT folder.
        /// </summary>
        /// <value>The order service XSLT folder.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string OrderServiceXsltFolder { get; private set; }

        /// <summary>
        ///     The app-relative path to the folder that temporary uploaded files are stored in.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string UploaderContentFolder { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether to show debug logging in the upload UI dialog.
        /// </summary>
        /// <value>
        ///     <c>true</c> if upload UI logging is enabled; otherwise, <c>false</c>.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public bool EnableUploadUiLogging { get; private set; }

        /// <summary>
        ///     Gets the host name of the middle tier REST service
        /// </summary>
        /// <value>
        ///     The name of the service host.
        /// </value>
        public Uri ServiceRootUri { get; private set; }

        /// <summary>
        ///     Gets the value of the role claim expected to identify the identity as a UL Employee.  Should be 'UL-Employee'
        /// </summary>
        /// <value>
        ///     The ul employee role claim value.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string UlEmployeeRoleClaimValue { get; private set; }

        /// <summary>
        ///     Gets the ID for the UL Company.  This is where some new users will added to.
        /// </summary>
        /// <value>
        ///     The ul company id.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Guid UlCompanyId { get; private set; }

        /// <summary>
        ///     Gets list of document types
        /// </summary>
        public DocumentType[] DocumentTypes { get; private set; }

        /// <summary>
        ///     Gets the industries.
        /// </summary>
        /// <value>
        ///     The industries.
        /// </value>
        public IDictionary<string, string> Industries { get; private set; }


        /// <summary>
        ///     Gets the task phase.
        /// </summary>
        /// <value>
        ///     The task phase.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public IDictionary<string, string> TaskPhase { get; private set; }

        /// <summary>
        ///     Gets the task progress.
        /// </summary>
        /// <value>
        ///     The task progress.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public IDictionary<string, string> TaskProgress { get; private set; }

        /// <summary>
        ///     Gets the task percent complete.
        /// </summary>
        /// <value>
        ///     The task percent complete.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public IDictionary<double, string> TaskPercentComplete { get; private set; }


	    /// <summary>
        ///     Gets the task review group email.
        /// </summary>
        /// <value>
        ///     The task review group email.
        /// </value>
	    // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string TaskReviewGroupEmail { get; private set; }

        /// <summary>
        ///     Gets the list of UL apps.
        /// </summary>
        /// <value>
        ///     The ul apps.
        /// </value>
        public IEnumerable<MenuItem> UlApps { get; private set; }

        /// <summary>
        ///     Gets the max favorites to show.
        /// </summary>
        /// <value>
        ///     The max favorites to show.
        /// </value>
        public int MaxFavoritesToShow { get; set; }

        /// <summary>
        ///     Gets or sets the google analytics 'tracking id'.
        /// </summary>
        /// <value>
        ///     The web analytics tracking id.
        /// </value>
        public string WebAnalyticsTrackingId { get; set; }

        /// <summary>
        ///     Gets or sets the google analytics config argument.
        /// </summary>
        /// <value>
        ///     The web analytics config.
        /// </value>
        public string WebAnalyticsConfig { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [web analytics turn on].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [web analytics turn on]; otherwise, <c>false</c>.
        /// </value>
        public bool WebAnalyticsEnabled { get; set; }

		/// <summary>
		/// Gets a value indicating whether to override the session duration the STS provides
		/// with a duration based on our local configuration.
		/// </summary>
		/// <value>
		/// <c>true</c> if [override STS session time]; otherwise, <c>false</c>.
		/// </value>
	    public bool ShouldOverrideStsSessionTime { get; set; }

        public int OverrideStsSessionTimeout { get; set; }
        
        /// <summary>
        /// Gets the labware.
        /// </summary>
        /// <value>
        /// The labware.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public LabwareSection Labware { get; set; }

        /// <summary>
        /// Gets the Dap.
        /// </summary>
        /// <value>
        /// The Dap.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public DapSection Dap { get; set; }

        /// <summary>
        /// Gets the Clp.
        /// </summary>
        /// <value>
        /// The Clp.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public ManageLabwareProjectSection ManageLabwareProject { get; set; }
        /// <summary>
        /// Gets the Eap.
        /// </summary>
        /// <value>
        /// The Eap.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public EapSection Eap { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable document caching].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable document caching]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableDocumentCaching { get; set; }

		/// <summary>
		/// Gets the document content service proxy.
		/// </summary>
		/// <value>The document content service proxy.</value>
	    public DocumentContentServiceProxySection DocumentContentServiceProxy { get; private set; }


		/// <summary>
		/// Gets or sets the sharePoint document edit URL.
		/// </summary>
		/// <value>
		/// The sharePoint document edit URL.
		/// </value>
		public string SharePointDocumentEditUrl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [enable document online edit].
		/// </summary>
		/// <value>
		/// <c>true</c> if [enable document online edit]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableDocumentOnlineEdit { get; set; }

        /// <summary>
        /// Gets or sets the freeform task type identifier.
        /// </summary>
        /// <value>
        /// The freeform task type identifier.
        /// </value>
        public Guid FreeformTaskTypeId { get; private set; }


		/// <summary>
		/// Gets or sets a value indicating whether this instance can preview document online.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can preview document online; otherwise, <c>false</c>.
		/// </value>
		public bool CanPreviewDocumentOnline { get; set; }
		/// <summary>
		/// Gets or sets the share point document preview URL.
		/// </summary>
		/// <value>
		/// The share point document preview URL.
		/// </value>
		public string SharePointDocumentPreviewUrl { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [enable redis temporary data support].
        /// </summary>
        /// <value><c>true</c> if [enable redis temporary data support]; otherwise, <c>false</c>.</value>
        public bool EnableRedisTempDataSupport { get; set; }
    }
}