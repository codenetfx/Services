using System;
using System.Configuration;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageFailedAzureBlobStorageLocatorProvider. This class cannot be inherited.
	/// </summary>
	public sealed class InboundMessageFailedAzureBlobStorageLocatorProvider : IAzureBlobStorageLocatorProvider
	{
		private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageFailedAzureBlobStorageLocatorProvider"/> class.
		/// </summary>
		/// <param name="azureBlobStorageRepositoryConfigurationSourceResolver">The azure BLOB storage repository configuration source resolver.</param>
		public InboundMessageFailedAzureBlobStorageLocatorProvider(IAzureBlobStorageRepositoryConfigurationSourceResolver azureBlobStorageRepositoryConfigurationSourceResolver)
		{
			_azureBlobStorageConfiguration = new AzureBlobStorageConfiguration
			{
				StorageConnectionString =
					ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString,
				Container =
					ConfigurationManager.AppSettings.GetValue("InboundMessage.Container.Failed",
						null),
				AzureBlobStorageRepositoryConfigurationSource =
					azureBlobStorageRepositoryConfigurationSourceResolver.Resolve("InboundMessage")
			};			
		}

		/// <summary>
		/// Fetches this instance.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AzureBlobStorageConfiguration.</returns>
		public AzureBlobStorageConfiguration FetchById(Guid? id = null)
		{
			return _azureBlobStorageConfiguration;
		}
	}
}