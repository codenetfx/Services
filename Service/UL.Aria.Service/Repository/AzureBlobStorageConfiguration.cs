namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureBlobStorageConfiguration.
	/// </summary>
	public class AzureBlobStorageConfiguration
	{
		/// <summary>
		/// Gets or sets the storage connection string.
		/// </summary>
		/// <value>The storage connection string.</value>
		public string StorageConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the container.
		/// </summary>
		/// <value>The container.</value>
		public string Container { get; set; }

		/// <summary>
		/// Gets or sets the azure BLOB storage repository configuration source.
		/// </summary>
		/// <value>The azure BLOB storage repository configuration source.</value>
		public IAzureBlobStorageRepositoryConfigurationSource AzureBlobStorageRepositoryConfigurationSource { get; set; }
	}
}