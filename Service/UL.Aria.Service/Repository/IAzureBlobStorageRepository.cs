using System.Collections.Generic;
using System.IO;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAzureBlobStorageRepository
	/// </summary>
	public interface IAzureBlobStorageRepository
	{
		/// <summary>
		/// Creates the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		void CreateContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration);

		/// <summary>
		/// Fetches the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <returns>AzureStorageContainer.</returns>
		AzureBlobStorageContainer FetchContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration);

		/// <summary>
		/// Deletes the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		void DeleteContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration);

		/// <summary>
		/// Deletes the container blobs by expire days.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="expireDays">The expire days.</param>
		void DeleteContainerBlobsByExpireDays(AzureBlobStorageConfiguration azureBlobStorageConfiguration, int expireDays);

		/// <summary>
		/// Fetches the specified inbound document container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns>AzureStorageBlobInternal.</returns>
		AzureStorageBlobInternal Fetch(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName);

		/// <summary>
		/// Saves the specified inbound document container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <param name="blobProperties">The BLOB properties.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>BlobProperties.</returns>
		BlobProperties Save(AzureBlobStorageConfiguration azureBlobStorageConfiguration, IDictionary<string, string> metadata, string blobName,
			BlobProperties blobProperties, Stream stream);

		/// <summary>
		/// Deletes the specified inbound document container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		void Delete(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName);

		/// <summary>
		/// Existses the specified azure storage configuration.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool Exists(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName);
	}
}