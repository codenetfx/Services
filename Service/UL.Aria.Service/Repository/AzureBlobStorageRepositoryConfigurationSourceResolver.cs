using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureBlobStorageRepositoryConfigurationSourceResolver.
	/// </summary>
	public class AzureBlobStorageRepositoryConfigurationSourceResolver : IAzureBlobStorageRepositoryConfigurationSourceResolver
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="AzureBlobStorageRepositoryConfigurationSourceResolver"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public AzureBlobStorageRepositoryConfigurationSourceResolver(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureBlobStorageRepositoryConfigurationSource.</returns>
		public IAzureBlobStorageRepositoryConfigurationSource Resolve(string name)
		{
			return _container.Resolve<IAzureBlobStorageRepositoryConfigurationSource>(name);
		}
	}
}