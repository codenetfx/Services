using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class AzureServiceBusQueueLocatorProviderResolver. This class cannot be inherited.
	/// </summary>
	public sealed class AzureServiceBusQueueLocatorProviderResolver : IAzureServiceBusQueueLocatorProviderResolver
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="AzureServiceBusQueueLocatorProviderResolver"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public AzureServiceBusQueueLocatorProviderResolver(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureServiceBusQueueLocatorProvider.</returns>
		public IAzureServiceBusQueueLocatorProvider Resolve(string name)
		{
			return _container.Resolve<IAzureServiceBusQueueLocatorProvider>(name);
		}
	}
}