﻿using System;
using System.Configuration;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageOrderMessageAzureBlobStorageLocatorProvider. This class cannot be inherited.
	/// </summary>
	public sealed class InboundMessageOrderMessageAzureBlobStorageLocatorProvider : IAzureBlobStorageLocatorProvider
	{
		// ReSharper disable once InconsistentNaming
		private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageOrderMessageAzureBlobStorageLocatorProvider"/> class.
		/// </summary>
		/// <param name="azureBlobStorageRepositoryConfigurationSourceResolver">The azure BLOB storage repository configuration source resolver.</param>
		public InboundMessageOrderMessageAzureBlobStorageLocatorProvider(IAzureBlobStorageRepositoryConfigurationSourceResolver azureBlobStorageRepositoryConfigurationSourceResolver)
		{
			_azureBlobStorageConfiguration = new AzureBlobStorageConfiguration
			{
				StorageConnectionString =
					ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString,
				Container =
					ConfigurationManager.AppSettings.GetValue("InboundMessage.Container.OrderMessages",
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