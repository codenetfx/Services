using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureBlobStorageHelper.
	/// </summary>
	public static class AzureBlobStorageHelper
	{
		/// <summary>
		/// Gets the BLOB.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blob">The BLOB.</param>
		/// <returns>CloudBlockBlob.</returns>
		public static CloudBlockBlob GetBlob(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blob)
		{
			var blobContainer = GetBlobContainer(azureBlobStorageConfiguration);
			return blobContainer.GetBlockBlobReference(blob);
		}

		/// <summary>
		/// Gets the BLOB container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <returns>CloudBlobContainer.</returns>
		public static CloudBlobContainer GetBlobContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration)
		{
			var blobClient = GetStorageAccount(azureBlobStorageConfiguration)
				.CreateCloudBlobClient();
			blobClient.DefaultRequestOptions = new BlobRequestOptions
			{
				ParallelOperationThreadCount = azureBlobStorageConfiguration.AzureBlobStorageRepositoryConfigurationSource.ParallelOperationThreadCount,
				ServerTimeout = azureBlobStorageConfiguration.AzureBlobStorageRepositoryConfigurationSource.ServerTimeOut,
				SingleBlobUploadThresholdInBytes = azureBlobStorageConfiguration.AzureBlobStorageRepositoryConfigurationSource.SingleUploadThresholdInBytes
			};
			return
				blobClient
					.GetContainerReference(azureBlobStorageConfiguration.Container.ToLower());
		}

		/// <summary>
		/// Gets the storage account.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <returns>CloudStorageAccount.</returns>
		public static CloudStorageAccount GetStorageAccount(AzureBlobStorageConfiguration azureBlobStorageConfiguration)
		{
			return CloudStorageAccount.Parse(azureBlobStorageConfiguration.StorageConnectionString);
		}
	}
}