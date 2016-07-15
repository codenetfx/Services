namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureServiceBusQueueConfiguration.
	/// </summary>
	public class AzureServiceBusQueueConfiguration
	{
		/// <summary>
		/// Gets or sets the service bus queue.
		/// </summary>
		/// <value>The service bus queue.</value>
		public string ServiceBusQueue { get; set; }

		/// <summary>
		/// Gets or sets the service bus connection string.
		/// </summary>
		/// <value>The service bus connection string.</value>
		public string ServiceBusConnectionString { get; set; }
	}
}