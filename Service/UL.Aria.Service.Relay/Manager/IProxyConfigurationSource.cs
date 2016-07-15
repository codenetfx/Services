using System;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyConfigurationSource
    {
        /// <summary>
        /// Gets the product service.
        /// </summary>
        /// <value>
        /// The product service.
        /// </value>
        Uri ProductService { get;}

        /// <summary>
        /// Gets the project service.
        /// </summary>
        /// <value>
        /// The project service.
        /// </value>
        Uri ProjectService { get; }

        /// <summary>
        /// Gets the aria service.
        /// </summary>
        /// <value>
        /// The aria service.
        /// </value>
        Uri AriaService { get; }

        /// <summary>
        /// Gets or sets the profile service.
        /// </summary>
        /// <value>
        /// The profile service.
        /// </value>
        Uri ProfileService { get; }

        /// <summary>
        /// Gets or sets the company service.
        /// </summary>
        /// <value>
        /// The company service.
        /// </value>
        Uri CompanyService { get; }

		/// <summary>
		/// Gets the document service.
		/// </summary>
		/// <value>The document service.</value>
		Uri DocumentService { get; }

		/// <summary>
		/// Gets the document content service.
		/// </summary>
		/// <value>The document content service.</value>
		Uri DocumentContentService { get; }
	}
}