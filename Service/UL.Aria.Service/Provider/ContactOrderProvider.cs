using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class ContactOrderProvider. This class cannot be inherited.
	/// </summary>
	public sealed class ContactOrderProvider : AzureProviderBase<ContactOrderDto>, IContactOrderProvider
	{
		private readonly IAzureServiceBusQueueRepository<ContactOrderDto> _contactAzureServiceBusQueueRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContactOrderProvider"/> class.
		/// </summary>
		/// <param name="contactAzureServiceBusQueueRepository">The contact azure service bus queue repository.</param>
		/// <param name="azureServiceBusQueueLocatorProviderResolver">The azure service bus queue locator provider resolver.</param>
		public ContactOrderProvider(IAzureServiceBusQueueRepository<ContactOrderDto> contactAzureServiceBusQueueRepository,
			IAzureServiceBusQueueLocatorProviderResolver azureServiceBusQueueLocatorProviderResolver)
			: base(null, azureServiceBusQueueLocatorProviderResolver, null, contactAzureServiceBusQueueRepository)
		{
			_contactAzureServiceBusQueueRepository = contactAzureServiceBusQueueRepository;
		}

		/// <summary>
		/// Queues the contact order.
		/// </summary>
		/// <param name="contactOrder">The contact order.</param>
		public void QueueContactOrder(ContactOrderDto contactOrder)
		{
			var azureServiceBusQueueConfiguration = GetAzureServiceBusQueueConfiguration("ContactOrder");
			_contactAzureServiceBusQueueRepository.CreateQueue(azureServiceBusQueueConfiguration);
			_contactAzureServiceBusQueueRepository.AddQueueMessage(azureServiceBusQueueConfiguration, contactOrder);
		}

		/// <summary>
		/// Creates the contact order queue.
		/// </summary>
		public void CreateContactOrderQueue()
		{
			var azureServiceBusQueueConfiguration = GetAzureServiceBusQueueConfiguration("ContactOrder");
			_contactAzureServiceBusQueueRepository.CreateQueue(azureServiceBusQueueConfiguration);
		}
	}
}