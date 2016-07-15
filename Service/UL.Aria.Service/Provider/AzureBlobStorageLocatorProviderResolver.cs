using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class AzureBlobStorageLocatorProviderResolver. This class cannot be inherited.
	/// </summary>
	public sealed class AzureBlobStorageLocatorProviderResolver : IAzureBlobStorageLocatorProviderResolver
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="AzureBlobStorageLocatorProviderResolver"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public AzureBlobStorageLocatorProviderResolver(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureBlobStorageLocatorProvider.</returns>
		public IAzureBlobStorageLocatorProvider Resolve(string name)
		{
			return _container.Resolve<IAzureBlobStorageLocatorProvider>(name);
		}
	}
}
