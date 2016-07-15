namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAzureServiceBusQueueRepository
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAzureServiceBusQueueRepository<T>
	{
		/// <summary>
		/// Adds the queue message.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <param name="message">The message.</param>
		void AddQueueMessage(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration, T message);

		/// <summary>
		/// Creates the queue.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		void CreateQueue(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration);

		/// <summary>
		/// Pops the queue message.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <returns>T.</returns>
		T PopQueueMessage(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration);

		/// <summary>
		/// Deletes the queue.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		void DeleteQueue(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration);

		/// <summary>
		/// Queues the exists.
		/// </summary>
		/// <param name="azureServiceBusQueueConfiguration">The azure service bus queue configuration.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool QueueExists(AzureServiceBusQueueConfiguration azureServiceBusQueueConfiguration);
	}
}