using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageProvider. This class cannot be inherited.
	/// </summary>
	public sealed class InboundMessageProvider : AzureProviderBase<InboundMessageDto>, IInboundMessageProvider
	{
		private readonly IAzureBlobStorageRepository _azureBlobStorageRepository;
		private readonly IAzureServiceBusQueueRepository<InboundMessageDto> _inboundMessageAzureServiceBusQueueRepository;
		private readonly IInboundMessageProviderConfigurationSource _inboundMessageProviderConfigurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageProvider" /> class.
		/// </summary>
		/// <param name="inboundMessageProviderConfigurationSource">The inbound message provider configuration source.</param>
		/// <param name="azureBlobStorageRepository">The azure BLOB storage repository.</param>
		/// <param name="azureBlobStorageLocatorProviderResolver">The azure BLOB storage locator provider resolver.</param>
		/// <param name="inboundMessageAzureServiceBusQueueRepository">The inbound message azure service bus queue repository.</param>
		/// <param name="azureServiceBusQueueLocatorProviderResolver">The azure service bus queue locator provider resolver.</param>
		public InboundMessageProvider(IInboundMessageProviderConfigurationSource inboundMessageProviderConfigurationSource,
			IAzureBlobStorageRepository azureBlobStorageRepository,
			IAzureBlobStorageLocatorProviderResolver azureBlobStorageLocatorProviderResolver,
			IAzureServiceBusQueueRepository<InboundMessageDto> inboundMessageAzureServiceBusQueueRepository,
			IAzureServiceBusQueueLocatorProviderResolver azureServiceBusQueueLocatorProviderResolver)
			: base(
				azureBlobStorageLocatorProviderResolver, azureServiceBusQueueLocatorProviderResolver, azureBlobStorageRepository,
				inboundMessageAzureServiceBusQueueRepository)
		{
			_inboundMessageProviderConfigurationSource = inboundMessageProviderConfigurationSource;
			_azureBlobStorageRepository = azureBlobStorageRepository;
			_inboundMessageAzureServiceBusQueueRepository = inboundMessageAzureServiceBusQueueRepository;
		}

		/// <summary>
		/// Saves the successful message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="metadata">The metadata.</param>
		public void SaveSuccessfulMessage(string messageId, string message, IDictionary<string, string> metadata = null)
		{
			SaveMessage(messageId, message, "InboundMessageSuccessful", metadata);
			DeleteNewMessage(messageId);
		}

		/// <summary>
		/// Saves the failed message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="metadata">The metadata.</param>
		public void SaveFailedMessage(string messageId, string message, IDictionary<string, string> metadata = null)
		{
			SaveMessage(messageId, message, "InboundMessageFailed", metadata);
		}

		/// <summary>
		/// Saves the order message.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <param name="orderMessage">The order message.</param>
		/// <param name="metadata">The metadata.</param>
		public void SaveOrderMessage(string orderNumber, string orderMessage, IDictionary<string, string> metadata = null)
		{
			SaveMessage(orderNumber, orderMessage, "InboundMessageOrderMessage", metadata);
			// ReSharper disable once PossibleNullReferenceException
			DeleteNewMessage(metadata["MessageId"]);
		}

		/// <summary>
		/// Saves the new message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <param name="message">The message.</param>
		/// <param name="metadata">The metadata.</param>
		public void SaveNewMessage(string messageId, string message, IDictionary<string, string> metadata = null)
		{
			SaveMessage(messageId, message, "InboundMessageNew", metadata);
		}

		/// <summary>
		/// Saves the new message and queues the notification queue message.
		/// </summary>
		/// <param name="inboundMessage">The inbound message.</param>
		/// <param name="message">The message.</param>
		/// <param name="metadata">The metadata.</param>
		public void QueueNewMessage(InboundMessageDto inboundMessage, string message,
			IDictionary<string, string> metadata = null)
		{
			var azureServiceBusQueueConfiguration = GetAzureServiceBusQueueConfiguration("InboundMessageNew");
			_inboundMessageAzureServiceBusQueueRepository.CreateQueue(azureServiceBusQueueConfiguration);
			SaveMessage(inboundMessage.MessageId, message, "InboundMessageNew", metadata);
			_inboundMessageAzureServiceBusQueueRepository.AddQueueMessage(azureServiceBusQueueConfiguration, inboundMessage);
		}

		/// <summary>
		/// Creates the new queue.
		/// </summary>
		public void CreateNewQueue()
		{
			var azureServiceBusQueueConfiguration = GetAzureServiceBusQueueConfiguration("InboundMessageNew");
			_inboundMessageAzureServiceBusQueueRepository.CreateQueue(azureServiceBusQueueConfiguration);
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns></returns>
		public string Ping()
		{
			return base.Ping("InboundMessageNew");
		}

		/// <summary>
		/// Fetches the order message.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <returns>System.String.</returns>
		public InboundMessageBlob FetchOrderMessage(string orderNumber)
		{
			return FetchMessage("InboundMessageOrderMessage", orderNumber);
		}

		/// <summary>
		/// Fetches the new message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <returns>InboundMessageBlob.</returns>
		public InboundMessageBlob FetchNewMessage(string messageId)
		{
			return FetchMessage("InboundMessageNew", messageId);
		}

		/// <summary>
		/// Deletes the order message.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		public void DeleteOrderMessage(string orderNumber)
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageOrderMessage");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.Delete(azureBlobStorageConfiguration, orderNumber);
		}

		/// <summary>
		/// Deletes the new message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		public void DeleteNewMessage(string messageId)
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageNew");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.Delete(azureBlobStorageConfiguration, messageId);
		}

		/// <summary>
		/// Deletes the successful message.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		public void DeleteSuccessfulMessage(string messageId)
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageSuccessful");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.Delete(azureBlobStorageConfiguration, messageId);
		}

		/// <summary>
		/// Cleanups the failed messages.
		/// </summary>
		public void CleanupFailedMessages()
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageFailed");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.DeleteContainerBlobsByExpireDays(azureBlobStorageConfiguration,
				_inboundMessageProviderConfigurationSource.FailedExpireDays);
		}

		/// <summary>
		/// Cleanups the order message.
		/// </summary>
		public void CleanupOrderMessages()
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageOrderMessage");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.DeleteContainerBlobsByExpireDays(azureBlobStorageConfiguration,
				_inboundMessageProviderConfigurationSource.OrderMessageExpireDays);
		}

		/// <summary>
		/// Cleanups the new messages.
		/// </summary>
		public void CleanupNewMessages()
		{
			var azureBlobStorageConfiguration = GetAzureBlobStorageConfiguration("InboundMessageNew");
			_azureBlobStorageRepository.CreateContainer(azureBlobStorageConfiguration);
			_azureBlobStorageRepository.DeleteContainerBlobsByExpireDays(azureBlobStorageConfiguration,
				_inboundMessageProviderConfigurationSource.NewExpireDays);
		}

		private InboundMessageBlob FetchMessage(string azureBlobStorageLocatorProviderName, string blobName)
		{
			var azureStorageBlob = FetchBlob(azureBlobStorageLocatorProviderName, blobName);
			if (azureStorageBlob == null) return null;
			var inboundMessageBlob = new InboundMessageBlob {Metadata = azureStorageBlob.Metadata};

			using (var stream = azureStorageBlob.Stream)
			{
				var streamReader = new StreamReader(stream);
				inboundMessageBlob.Message = streamReader.ReadToEnd();
			}

			return inboundMessageBlob;
		}

		private void SaveMessage(string messageId, string message, string azureBlobStorageLocatorProviderName,
			IDictionary<string, string> metadata)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(message);
			using (var memoryStream = new MemoryStream(bytes))
			{
				SaveBlob(azureBlobStorageLocatorProviderName, messageId, "text/xml", metadata, memoryStream);
			}
		}
	}
}