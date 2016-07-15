using System;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IAzureServiceBusQueueLocatorProvider
	/// </summary>
	public interface IAzureServiceBusQueueLocatorProvider
	{
		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AzureServiceBusQueueConfiguration.</returns>
		AzureServiceBusQueueConfiguration FetchById(Guid? id = null);
	}
}