using System;

namespace UL.Aria.Service.Configuration
{
	/// <summary>
	/// Represents the configuration settings used by the aria portal
	/// </summary>
	public interface IServiceConfiguration
	{
		/// <summary>
		///     Gets the root URI to the aria web portal
		/// </summary>
		/// <value>
		///     The aria root URI.
		/// </value>
		Uri AriaRootUri { get; }

		/// <summary>
		/// Gets the portal admin's email address.
		/// </summary>
		/// <value>
		/// Portal admin's email address.
		/// </value>
		string PortalAdminEmail { get; }

		/// <summary>
		/// Gets the From email address to use by default for emails sent by Aria
		/// </summary>
		/// <value>
		/// The default sender email.
		/// </value>
		string DefaultSenderEmail { get; }

		/// <summary>
		///     Gets the Aria customer support email address.
		/// </summary>
		/// <value>
		///     The contact us from email.
		/// </value>
		string CustomerSupportEmail { get; }

		/// <summary>
		///     Gets the Aria product support email address.  Will default to the CustomerSupportEmail.
		/// </summary>
		/// <value>
		///     The contact us from email.
		/// </value>
		string ProductSupportEmail { get; }

		/// <summary>
		///     Gets the email address to BCC on all emails sent from Aria.  This can be blank.
		/// </summary>
		/// <value>
		///     The global BCC email.
		/// </value>
		string GlobalBccEmail { get; }

		/// <summary>
		/// Gets the ul company unique identifier.
		/// </summary>
		/// <value>
		/// The ul company unique identifier.
		/// </value>
		Guid UlCompanyId { get; }

		/// <summary>
		/// Gets the storage connection string.
		/// </summary>
		/// <value>
		/// The storage connection string.
		/// </value>
		string StorageConnectionString { get; }

		/// <summary>
		/// Gets or sets the name of the storage container.
		/// </summary>
		/// <value>
		/// The name of the storage container.
		/// </value>
		string ScratchSpaceRootPath { get; }

		/// <summary>
		/// Gets the storage mode.
		/// </summary>
		/// <value>
		/// The storage mode.
		/// </value>
		ScratchSpaceStorageOption ScratchSpaceStorageOption { get; }

		/// <summary>
		/// Gets the scratch space expiration.
		/// </summary>
		/// <value>
		/// The scratch space expiration.
		/// </value>
		double ScratchSpaceExpiration { get; }

		/// <summary>
		/// Gets or sets the redis cache connection string.
		/// </summary>
		/// <value>
		/// The redis cache connection string.
		/// </value>
		string RedisCacheConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the redis item default expiry minimum.
		/// </summary>
		/// <value>
		/// The redis item default expiry minimum.
		/// </value>
		int RedisItemDefaultExpiryMin { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is redis cache enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is redis cache enabled; otherwise, <c>false</c>.</value>
		bool IsRedisCacheEnabled { get; }

	    /// <summary>
	    /// Gets or sets all business unit identifier.
	    /// </summary>
	    /// <value>
	    /// All business unit identifier.
	    /// </value>
	    Guid AllBusinessUnitId { get; set; }

        /// <summary>
        /// Gets the freeform task type identifier.
        /// </summary>
        /// <value>
        /// The freeform task type identifier.
        /// </value>
        Guid FreeformTaskTypeId { get; }

        /// <summary>
        /// Gets the task review group email.
        /// </summary>
        /// <value>
        /// The task review group email.
        /// </value>
        string TaskReviewGroupEmail { get; }
	}
}