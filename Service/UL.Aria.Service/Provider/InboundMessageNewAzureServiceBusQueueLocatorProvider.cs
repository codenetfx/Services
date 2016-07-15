using System;
using System.Configuration;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class InboundMessageNewAzureServiceBusQueueLocatorProvider. This class cannot be inherited.
	/// </summary>
	public sealed class InboundMessageNewAzureServiceBusQueueLocatorProvider : IAzureServiceBusQueueLocatorProvider
	{
		private readonly AzureServiceBusQueueConfiguration _azureServiceBusQueueConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="InboundMessageNewAzureServiceBusQueueLocatorProvider"/> class.
		/// </summary>
		public InboundMessageNewAzureServiceBusQueueLocatorProvider()
		{
			_azureServiceBusQueueConfiguration = new AzureServiceBusQueueConfiguration
			{
				ServiceBusQueue = ConfigurationManager.AppSettings.GetValue("InboundMessage.QueueName", null),
				ServiceBusConnectionString = ConfigurationManager.ConnectionStrings["InboundMessageServicebus"].ConnectionString
			};
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AzureServiceBusQueueConfiguration.</returns>
		public AzureServiceBusQueueConfiguration FetchById(Guid? id = null)
		{
			return _azureServiceBusQueueConfiguration;
		}
	}
}