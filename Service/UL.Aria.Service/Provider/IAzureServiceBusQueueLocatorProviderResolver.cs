namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IAzureServiceBusQueueLocatorProviderResolver
	/// </summary>
	public interface IAzureServiceBusQueueLocatorProviderResolver
	{
		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureServiceBusQueueLocatorProvider.</returns>
		IAzureServiceBusQueueLocatorProvider Resolve(string name);
	}
}