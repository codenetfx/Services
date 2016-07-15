using System;
using System.Configuration;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Configuration
{
	/// <summary>
	///     Represents the configuration settings used by the service
	/// </summary>
	public class ServiceConfiguration : IServiceConfiguration
	{
		private readonly bool _isRedisCacheEnabled;

		/// <summary>
		///     Initializes a new instance of the <see cref="ServiceConfiguration" /> class.
		/// </summary>
		public ServiceConfiguration()
		{
			var settings = ConfigurationManager.AppSettings;

			//
			// email
			//
			PortalAdminEmail = settings.GetValue("UL.Email.PortalAdmin", "portalservices@tempuri.org");
			DefaultSenderEmail = settings.GetValue("UL.Email.DefaultSender", "portalservices@tempuri.org");
			CustomerSupportEmail = settings.GetValue("UL.Email.CustomerSupportEmail", "portalservices@tempuri.org");
			ProductSupportEmail = settings.GetValue("UL.Email.ProductSupportEmail", CustomerSupportEmail);
			GlobalBccEmail = settings.GetValue("UL.Email.GlobalBccEmail", string.Empty);
            TaskReviewGroupEmail = settings.GetValue("UL.Email.TaskReviewGroupEmail", "ReviewPortalTasks@ul.com");

			//
			// scratch space
			//
			StorageConnectionString = settings.GetValue("UL.StorageConnectionString", string.Empty);
			ScratchSpaceRootPath = settings.GetValue("UL.ScratchSpaceRootPath", "AriaScratchSpace");
			ScratchSpaceStorageOption = settings.GetValue("UL.ScratchSpaceStorageOption", ScratchSpaceStorageOption.Default);
			ScratchSpaceExpiration = settings.GetValue("UL.ScratchSpaceExpirationPeriod", 48*60);

			//
			// other
			//
			UlCompanyId = settings.GetValue("UL.UlCompanyId", Guid.Parse("46F65EA8-913D-4F36-9E28-89951E7CE8EF"));
			var ariaRoot = settings.GetValue("UL.AriaRootUri", "http://portal:801/");
			AriaRootUri = new Uri(ariaRoot);

			//
			//RedisCacheManager
			//
			RedisCacheConnectionString = settings.GetValue("UL.RedisCacheConnectionString", "localhost:6379");
			RedisItemDefaultExpiryMin = settings.GetValue("UL.RedisItemExpiry", 0);
			_isRedisCacheEnabled = settings.GetValue("UL.RedisCacheEnabled", true);

            AllBusinessUnitId = settings.GetValue("UL.AllBusinessUnitId", Guid.Parse("AF18CEDA-D732-E411-80E5-000C29EB7D41"));

            // Default to the 'Basic' task type ID.
            FreeformTaskTypeId = settings.GetValue("UL.FreeformTaskTypeId", Guid.Parse("BEE714E0-FF7E-41B7-9B25-4D628F4FEF45"));
		}

		/// <summary>
		///     Gets the hostname and port of the Aria home page
		/// </summary>
		public Uri AriaRootUri { get; private set; }

		/// <summary>
		///     Gets the Aria customer support email address.
		/// </summary>
		/// <value>
		///     The contact us from email.
		/// </value>
		public string CustomerSupportEmail { get; private set; }

		/// <summary>
		/// Gets the Aria product support email address.  Will default to the CustomerSupportEmail.
		/// </summary>
		/// <value>
		/// The contact us from email.
		/// </value>
		public string ProductSupportEmail { get; private set; }

		/// <summary>
		///     Gets the email address to BCC on all emails sent from Aria.  This can be blank.
		/// </summary>
		/// <value>
		///     The global BCC email.
		/// </value>
		public string GlobalBccEmail { get; private set; }

		/// <summary>
		/// Gets the From email address to use by default for emails sent by Aria
		/// </summary>
		/// <value>
		/// The default sender email.
		/// </value>
		public string DefaultSenderEmail { get; private set; }

		/// <summary>
		/// Gets the email address of portal admin.
		/// </summary>
		/// <value>
		/// Portal admin's email address.
		/// </value>
		public string PortalAdminEmail { get; private set; }

		/// <summary>
		/// Gets the ul company unique identifier.
		/// </summary>
		/// <value>
		/// The ul company unique identifier.
		/// </value>
		public Guid UlCompanyId { get; private set; }

		/// <summary>
		/// Gets the storage connection string.
		/// </summary>
		/// <value>
		/// The storage connection string.
		/// </value>
		public string StorageConnectionString { get; private set; }

		/// <summary>
		/// Gets or sets the name of the storage container.
		/// </summary>
		/// <value>
		/// The name of the storage container.
		/// </value>
		public string ScratchSpaceRootPath { get; set; }

		/// <summary>
		/// Gets the storage mode.
		/// </summary>
		/// <value>
		/// The storage mode.
		/// </value>
		public ScratchSpaceStorageOption ScratchSpaceStorageOption { get; private set; }

		/// <summary>
		/// Gets the scratch space expiration.
		/// </summary>
		/// <value>
		/// The scratch space expiration.
		/// </value>
		public double ScratchSpaceExpiration { get; private set; }

		/// <summary>
		/// Gets or sets the redis cache connection string.
		/// </summary>
		/// <value>
		/// The redis cache connection string.
		/// </value>
		public string RedisCacheConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the redis item default expiry minimum.
		/// </summary>
		/// <value>
		/// The redis item default expiry minimum.
		/// </value>
		public int RedisItemDefaultExpiryMin { get; set; }

		/// <summary>
		/// Gets or sets the is redis cache enabled.
		/// </summary>
		/// <value>The is redis cache enabled.</value>
		public bool IsRedisCacheEnabled
		{
			get { return _isRedisCacheEnabled; }
		}

        /// <summary>
        /// Gets or sets all business unit identifier.
        /// </summary>
        /// <value>
        /// All business unit identifier.
        /// </value>
        public Guid AllBusinessUnitId { get; set; }

        /// <summary>
        /// Gets or sets the freeform task type identifier.
        /// </summary>
        /// <value>
        /// The freeform task type identifier.
        /// </value>
        public Guid FreeformTaskTypeId { get; private set; }

        /// <summary>
        /// Gets the task review group email.
        /// </summary>
        /// <value>
        /// The task review group email.
        /// </value>
        public string TaskReviewGroupEmail { get; private set; }
	}
}