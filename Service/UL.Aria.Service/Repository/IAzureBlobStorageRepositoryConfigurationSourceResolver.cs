namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAzureBlobStorageRepositoryConfigurationSourceResolver
	/// </summary>
	public interface IAzureBlobStorageRepositoryConfigurationSourceResolver
	{
		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureBlobStorageRepositoryConfigurationSource.</returns>
		IAzureBlobStorageRepositoryConfigurationSource Resolve(string name);
	}
}