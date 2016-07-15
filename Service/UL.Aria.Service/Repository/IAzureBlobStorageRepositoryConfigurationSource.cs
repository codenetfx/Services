using System;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface IAzureBlobStorageRepositoryConfigurationSource
	/// </summary>
	public interface IAzureBlobStorageRepositoryConfigurationSource
	{
		/// <summary>
		/// Gets the parallel operation thread count.
		/// </summary>
		/// <value>The parallel operation thread count.</value>
		int? ParallelOperationThreadCount { get; }

		/// <summary>
		/// Gets the server time out.
		/// </summary>
		/// <value>The server time out.</value>
		TimeSpan? ServerTimeOut { get; }

		/// <summary>
		/// Gets the single upload threshold in bytes.
		/// </summary>
		/// <value>The single upload threshold in bytes.</value>
		long? SingleUploadThresholdInBytes { get; }
	}
}