using System;
using System.Configuration;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class ContactOrderAzureServiceBusQueueLocatorProvider. This class cannot be inherited.
	/// </summary>
	public sealed class ContactOrderAzureServiceBusQueueLocatorProvider : IAzureServiceBusQueueLocatorProvider
	{
		private readonly AzureServiceBusQueueConfiguration _azureServiceBusQueueConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContactOrderAzureServiceBusQueueLocatorProvider"/> class.
		/// </summary>
		public ContactOrderAzureServiceBusQueueLocatorProvider()
		{
			_azureServiceBusQueueConfiguration = new AzureServiceBusQueueConfiguration
			{
				ServiceBusQueue = ConfigurationManager.AppSettings.GetValue("ContactOrder.QueueName", null),
				ServiceBusConnectionString = ConfigurationManager.ConnectionStrings["ContactOrderServicebus"].ConnectionString
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