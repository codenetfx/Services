using System.Diagnostics.CodeAnalysis;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureServiceBusQueueRepositoryBase.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[ExcludeFromCodeCoverage]
	public abstract class AzureServiceBusQueueRepositoryBase<T> : IAzureServiceBusQueueRepository<T>
	{
		/// <summary>
		/// Adds the queue message.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <param name="message">The message.</param>
		public void AddQueueMessage(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration, T message)
		{
			QueueClient client =
				QueueClient.CreateFromConnectionString(azureServiceBusQueueConfiguration.ServiceBusConnectionString,
					azureServiceBusQueueConfiguration.ServiceBusQueue);
			client.Send(new BrokeredMessage(message));
		}

		/// <summary>
		/// Creates the queue.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		public void CreateQueue(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration)
		{
			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(azureServiceBusQueueConfiguration.ServiceBusConnectionString);

			if (!namespaceManager.QueueExists(azureServiceBusQueueConfiguration.ServiceBusQueue))
			{
				namespaceManager.CreateQueue(azureServiceBusQueueConfiguration.ServiceBusQueue);
			}
		}

		/// <summary>
		/// Pops the queue message. For integration testing purposes only.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <returns>T.</returns>
		public T PopQueueMessage(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration)
		{
			QueueClient client =
				QueueClient.CreateFromConnectionString(azureServiceBusQueueConfiguration.ServiceBusConnectionString,
					azureServiceBusQueueConfiguration.ServiceBusQueue);
			var brokeredMessage = client.Receive();
			var inboundMessage = brokeredMessage.GetBody<T>();
			client.Complete(brokeredMessage.LockToken);
			return inboundMessage;
		}

		/// <summary>
		/// Deletes the queue. For integration testing purposes only.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		public void DeleteQueue(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration)
		{
			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(azureServiceBusQueueConfiguration.ServiceBusConnectionString);

			if (namespaceManager.QueueExists(azureServiceBusQueueConfiguration.ServiceBusQueue))
			{
				namespaceManager.DeleteQueue(azureServiceBusQueueConfiguration.ServiceBusQueue);
			}
		}

		/// <summary>
		/// Queues the exists.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool QueueExists(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration)
		{
			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(azureServiceBusQueueConfiguration.ServiceBusConnectionString);
			return namespaceManager.QueueExists(azureServiceBusQueueConfiguration.ServiceBusQueue);
		}
	}
}