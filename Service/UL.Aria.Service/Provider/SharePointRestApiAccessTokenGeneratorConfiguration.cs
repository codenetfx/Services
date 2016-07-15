using System.Configuration;
using System.Security.Cryptography.X509Certificates;

using UL.Iam.Common.OAuth;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Configuration class for <see cref="SharePointRestApiAccessTokenGenerator" />
    /// </summary>
    public class SharePointRestApiAccessTokenGeneratorConfiguration :
        ISharePointRestApiAccessTokenGeneratorConfiguration
    {
        /// <summary>
        ///     Gets the search service OAuth client id.
        /// </summary>
        /// <value>
        ///     The search service OAuth client id.
        /// </value>
        public string ClientId
        {
            get { return ConfigurationManager.AppSettings["UL.Sharepoint.SearchService.OAuthClientId"]; }
        }


        /// <summary>
        ///     Gets the search service OAuth site id.
        /// </summary>
        /// <value>
        ///     The search service OAuth site id.
        /// </value>
        public string SiteId
        {
            get { return ConfigurationManager.AppSettings["UL.Sharepoint.SearchService.OAuthSiteId"]; }
        }

        /// <summary>
        ///     Gets or sets the trusted certificate thumprint.
        /// </summary>
        /// <value>
        ///     The trusted certificate thumprint.
        /// </value>
        public string TrustedCertificateThumprint
        {
            get { return ConfigurationManager.AppSettings["UL.Sharepoint.SearchService.TrustedX509CertificateThumbprint"]; }
        }

        /// <summary>
        ///     Gets or sets the name of the trusted certificate store.
        /// </summary>
        /// <value>
        ///     The name of the trusted certificate store.
        /// </value>
        public StoreName TrustedCertificateStoreName
        {
            get { return StoreName.My; }
        }

        /// <summary>
        ///     Gets or sets the trusted certificate location.
        /// </summary>
        /// <value>
        ///     The trusted certificate location.
        /// </value>
        public StoreLocation TrustedCertificateLocation
        {
            get { return StoreLocation.LocalMachine; }
        }
    }
}