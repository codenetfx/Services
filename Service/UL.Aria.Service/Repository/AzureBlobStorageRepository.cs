using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureBlobStorageRepository.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class AzureBlobStorageRepository : IAzureBlobStorageRepository
	{
		/// <summary>
		/// Creates the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		public void CreateContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration)
        {
			var blobContainer = AzureBlobStorageHelper.GetBlobContainer(azureBlobStorageConfiguration);
            blobContainer.CreateIfNotExists();
        }

		/// <summary>
		/// Fetches the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <returns>AzureStorageContainer.</returns>
		public AzureBlobStorageContainer FetchContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration)
		{
			var azureStorageContainer = new AzureBlobStorageContainer();
			var blobContainer = AzureBlobStorageHelper.GetBlobContainer(azureBlobStorageConfiguration);
            blobContainer.FetchAttributes();
			azureStorageContainer.LastModified = blobContainer.Properties.LastModified.GetValueOrDefault().DateTime.ToUniversalTime();
			return azureStorageContainer;
        }

		/// <summary>
		/// Deletes the container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
 		public void DeleteContainer(AzureBlobStorageConfiguration azureBlobStorageConfiguration)
        {
			var blobContainer = AzureBlobStorageHelper.GetBlobContainer(azureBlobStorageConfiguration);
            blobContainer.DeleteIfExists();
        }

		/// <summary>
		/// Deletes the container blobs by expire days.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="expireDays">The expire days.</param>
		public void DeleteContainerBlobsByExpireDays(AzureBlobStorageConfiguration azureBlobStorageConfiguration, int expireDays)
        {
			var container = AzureBlobStorageHelper.GetBlobContainer(azureBlobStorageConfiguration);
            var blobs =
                container.ListBlobs("", true)
                    .OfType<CloudBlockBlob>()
                // ReSharper disable once PossibleInvalidOperationException
					.Where(b => (DateTime.UtcNow.AddDays(expireDays * -1) > b.Properties.LastModified.Value.DateTime.ToUniversalTime()))
                    .ToList();

            foreach (var blob in blobs)
            {
                var cloudBlob = container.GetBlockBlobReference(blob.Name);
                cloudBlob.DeleteIfExists();
            }
        }

		/// <summary>
		/// Fetches the stream.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns>AzureStorageBlobInternal.</returns>
		public AzureStorageBlobInternal Fetch(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName)
        {
			var blob = AzureBlobStorageHelper.GetBlob(azureBlobStorageConfiguration, blobName);
			blob.FetchAttributes();
			return new AzureStorageBlobInternal
			{
				Name = blob.Name,
				Uri = blob.Uri,
				ContentType = blob.Properties.ContentType,
				HashValue = blob.Properties.ContentMD5,
				LastModified = blob.Properties.LastModified.GetValueOrDefault().DateTime.ToUniversalTime(),
				Stream = blob.OpenRead(),
				Metadata = blob.Metadata
			};
        }

		/// <summary>
		/// Saves the blob and it's corresponding properties and metadata.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <param name="blobProperties">The BLOB properties.</param>
		/// <param name="stream">The stream.</param>
		public BlobProperties Save(AzureBlobStorageConfiguration azureBlobStorageConfiguration, IDictionary<string, string> metadata,
            string blobName, BlobProperties blobProperties, Stream stream)
        {
			var blob = AzureBlobStorageHelper.GetBlob(azureBlobStorageConfiguration, blobName);
            blob.UploadFromStream(stream);
            blob.FetchAttributes();
			blobProperties.Size = blob.Properties.Length;
            blob.Properties.ContentType = blobProperties.ContentType;
            blob.SetProperties();
            if (metadata != null)
            {
                foreach (var metadataPair in metadata)
                {
                    blob.Metadata.Add(metadataPair.Key, metadataPair.Value);
                }
            }
            blob.SetMetadata();
			return blobProperties;
        }

		/// <summary>
		/// Deletes the specified inbound message container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		public void Delete(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName)
		{
			if (!string.IsNullOrWhiteSpace(blobName))
			{
				try
				{
					var blob = AzureBlobStorageHelper.GetBlob(azureBlobStorageConfiguration, blobName);
					blob.DeleteIfExists();
				}
				// ReSharper disable once EmptyGeneralCatchClause
				catch
				{
				}
			}
		}

		/// <summary>
		/// Existses the specified inbound message container.
		/// </summary>
		/// <param name="azureBlobStorageConfiguration">The azure BLOB storage configuration.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Exists(AzureBlobStorageConfiguration azureBlobStorageConfiguration, string blobName)
        {
			var blob = AzureBlobStorageHelper.GetBlob(azureBlobStorageConfiguration, blobName);
            return blob.Exists();
        }
    }
}