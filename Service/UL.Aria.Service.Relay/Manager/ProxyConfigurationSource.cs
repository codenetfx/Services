using System;
using System.Configuration;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Configuration information.
    /// </summary>
    public class ProxyConfigurationSource : IProxyConfigurationSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ProxyConfigurationSource()
        {
            string appSetting = ConfigurationManager.AppSettings["UL.ServiceRootUri"];
            if (string.IsNullOrWhiteSpace(appSetting))
                appSetting = "http://ariasvc:802/";
            var serviceRoot = new Uri(appSetting);
            ProductService = new Uri(serviceRoot, "ProductDetail");
            ProjectService = new Uri(serviceRoot, "Project");
            ProfileService = new Uri(serviceRoot, "Profile");
            CompanyService = new Uri(serviceRoot, "Company");
            DocumentService = new Uri(serviceRoot, "Documents");
            DocumentContentService = new Uri(serviceRoot, "DocumentContent");
            AriaService = new Uri(serviceRoot, "");
        }
        /// <summary>
        /// Gets the product service.
        /// </summary>
        /// <value>
        /// The product service.
        /// </value>
        public Uri ProductService { get; private set; }

        /// <summary>
        /// Gets the project service.
        /// </summary>
        /// <value>
        /// The project service.
        /// </value>
        public Uri ProjectService { get; private set; }
        /// <summary>
        /// Gets the aria service.
        /// </summary>
        /// <value>
        /// The aria service.
        /// </value>
        public Uri AriaService { get; private set; }

        /// <summary>
        /// Gets or sets the profile service.
        /// </summary>
        /// <value>
        /// The profile service.
        /// </value>
        public Uri ProfileService { get; private set; }

        /// <summary>
        /// Gets or sets the company service.
        /// </summary>
        /// <value>
        /// The company service.
        /// </value>
        public Uri CompanyService { get; private set; }

		/// <summary>
		/// Gets the document service.
		/// </summary>
		/// <value>The document service.</value>
	    public Uri DocumentService { get; private set; }

		/// <summary>
		/// Gets the document content service.
		/// </summary>
		/// <value>The document content service.</value>
		public Uri DocumentContentService { get; private set; }
    }
}