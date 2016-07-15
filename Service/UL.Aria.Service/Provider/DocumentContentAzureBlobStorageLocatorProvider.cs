using System;
using System.Configuration;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class DocumentContentAzureBlobStorageLocatorProvider. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentContentAzureBlobStorageLocatorProvider : IAzureBlobStorageLocatorProvider
	{
		private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentContentAzureBlobStorageLocatorProvider"/> class.
		/// </summary>
		/// <param name="azureBlobStorageRepositoryConfigurationSourceResolver">The azure BLOB storage repository configuration source resolver.</param>
		public DocumentContentAzureBlobStorageLocatorProvider(IAzureBlobStorageRepositoryConfigurationSourceResolver azureBlobStorageRepositoryConfigurationSourceResolver)
		{
			_azureBlobStorageConfiguration = new AzureBlobStorageConfiguration
			{
				StorageConnectionString =
					ConfigurationManager.AppSettings.GetValue("UL.Aria.Service.DocumentContent.StorageConnectionString",
						"UseDevelopmentStorage=true"),
				Container =
					ConfigurationManager.AppSettings.GetValue("UL.Aria.Service.DocumentContent.StorageContainerName",
						"11111111-1111-1111-1111-9c8aaa528b65"),
				AzureBlobStorageRepositoryConfigurationSource =
					azureBlobStorageRepositoryConfigurationSourceResolver.Resolve("DocumentContent")
			};			
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AzureBlobStorageConfiguration.</returns>
		public AzureBlobStorageConfiguration FetchById(Guid? id)
		{
			return _azureBlobStorageConfiguration;
		}
	}
}