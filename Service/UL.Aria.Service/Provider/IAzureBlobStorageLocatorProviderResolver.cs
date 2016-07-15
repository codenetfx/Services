namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IAzureBlobStorageLocatorProviderResolver
	/// </summary>
	public interface IAzureBlobStorageLocatorProviderResolver
	{
		/// <summary>
		/// Resolves the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>IAzureBlobStorageLocatorProvider.</returns>
		IAzureBlobStorageLocatorProvider Resolve(string name);
	}
}