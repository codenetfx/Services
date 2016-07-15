using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class AzureProviderBase.
	/// </summary>
	public abstract class AzureProviderBase<T>
	{
		private readonly IAzureBlobStorageLocatorProviderResolver _azureBlobStorageLocatorProviderResolver;
		private readonly IAzureServiceBusQueueLocatorProviderResolver _azureServiceBusQueueLocatorProviderResolver;
		private readonly IAzureBlobStorageRepository _azureBlobStorageRepository;
		private readonly IAzureServiceBusQueueRepository<T> _azureServiceBusQueueRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="AzureProviderBase{T}"/> class.
		/// </summary>
		/// <param name="azureBlobStorageLocatorProviderResolver">The azure BLOB storage locator provider resolver.</param>
		/// <param name="azureServiceBusQueueLocatorProviderResolver">The azure service bus queue locator provider resolver.</param>
		/// <param name="azureBlobStorageRepository">The azure BLOB storage repository.</param>
		/// <param name="azureServiceBusQueueRepository">The azure service bus queue repository.</param>
		protected AzureProviderBase(IAzureBlobStorageLocatorProviderResolver azureBlobStorageLocatorProviderResolver,
			IAzureServiceBusQueueLocatorProviderResolver azureServiceBusQueueLocatorProviderResolver,
			IAzureBlobStorageRepository azureBlobStorageRepository,
			IAzureServiceBusQueueRepository<T> azureServiceBusQueueRepository)
		{
			_azureBlobStorageLocatorProviderResolver = azureBlobStorageLocatorProviderResolver;
			_azureServiceBusQueueLocatorProviderResolver = azureServiceBusQueueLocatorProviderResolver;
			_azureBlobStorageRepository = azureBlobStorageRepository;
			_azureServiceBusQueueRepository = azureServiceBusQueueRepository;
		}

		/// <summary>
		/// Fetches the BLOB.
		/// </summary>
		/// <param name="azureBlobStorageLocatorProviderName">Name of the azure BLOB storage locator provider.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <returns>AzureStorageBlobInternal.</returns>
		protected AzureStorageBlobInternal FetchBlob(string azureBlobStorageLocatorProviderName, string blobName)
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration(azureBlobStorageLocatorProviderName);
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			return _azureBlobStorageRepository.Exists(azureBlobStorageConfiguration, blobName) ? _azureBlobStorageRepository.Fetch(azureBlobStorageConfiguration, blobName) : null;
		}

		/// <summary>
		/// Saves the BLOB.
		/// </summary>
		/// <param name="azureBlobStorageLocatorProviderName">Name of the azure BLOB storage locator provider.</param>
		/// <param name="blobName">Name of the BLOB.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="metadata">The metadata.</param>
		/// <param name="stream">The stream.</param>
		protected void SaveBlob(string azureBlobStorageLocatorProviderName, string blobName, string contentType, IDictionary<string, string> metadata, Stream stream)
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration(azureBlobStorageLocatorProviderName);
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.Save(azureBlobStorageConfiguration, metadata, blobName,
				new BlobProperties { ContentType = contentType }, stream);
		}

		/// <summary>
		/// Pings the specified azure BLOB storage locator provider name.
		/// </summary>
		/// <param name="azureBlobStorageLocatorProviderName">Name of the azure BLOB storage locator provider.</param>
		/// <returns>System.String.</returns>
		protected string Ping(string azureBlobStorageLocatorProviderName)
		{
			try
			{
				var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration(azureBlobStorageLocatorProviderName);
				_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
				_azureBlobStorageRepository.FetchContainer(azureBlobStorageConfiguration);
				var azureServiceBusQueueConfiguration = GetAzureServiceBusQueueConfiguration(azureBlobStorageLocatorProviderName);
				_azureServiceBusQueueRepository.CreateQueue(azureServiceBusQueueConfiguration);
				_azureServiceBusQueueRepository.QueueExists(azureServiceBusQueueConfiguration);
                return true.ToString();
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		/// <summary>
		/// Gets the azure BLOB storage configuration.
		/// </summary>
		/// <param name="azureBlobStorageLocatorProviderName">Name of the azure BLOB storage locator provider.</param>
		/// <returns>AzureBlobStorageConfiguration.</returns>
		protected AzureBlobStorageConfiguration GetAzureBlobStorageConfiguration(string azureBlobStorageLocatorProviderName)
		{
			var azureBlobStorageLocatorProvider =
				_azureBlobStorageLocatorProviderResolver.Resolve(azureBlobStorageLocatorProviderName);
			return azureBlobStorageLocatorProvider.FetchById();
		}

		/// <summary>
		/// Gets the azure service bus queue configuration.
		/// </summary>
		/// <param name="azureServiceBusQueueLocatorProviderName">Name of the azure service bus queue locator provider.</param>
		/// <returns>AzureServiceBusQueueConfiguration.</returns>
		protected AzureServiceBusQueueConfiguration GetAzureServiceBusQueueConfiguration(
			string azureServiceBusQueueLocatorProviderName)
		{
			var azureServiceBusQueueLocatorProvider =
				_azureServiceBusQueueLocatorProviderResolver.Resolve(azureServiceBusQueueLocatorProviderName);
			return azureServiceBusQueueLocatorProvider.FetchById();
		}
	}
}