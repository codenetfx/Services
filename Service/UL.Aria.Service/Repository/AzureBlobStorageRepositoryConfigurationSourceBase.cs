using System;
using System.Configuration;

using UL.Enterprise.Foundation;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class AzureBlobStorageConfigurationSourceBase.
	/// </summary>
	public abstract class AzureBlobStorageRepositoryConfigurationSourceBase :
		IAzureBlobStorageRepositoryConfigurationSource
	{
		// ReSharper disable once InconsistentNaming
		private readonly int? _parallelOperationThreadCount;
		// ReSharper disable once InconsistentNaming
		private readonly TimeSpan? _serverTimeOut;
		// ReSharper disable once InconsistentNaming
		private readonly long? _singleUploadThresholdInBytes;

		/// <summary>
		/// Initializes a new instance of the <see cref="AzureBlobStorageRepositoryConfigurationSourceBase"/> class.
		/// </summary>
		protected AzureBlobStorageRepositoryConfigurationSourceBase()
		{
			_parallelOperationThreadCount =
				ConfigurationManager.AppSettings.GetValue<int?>(
					// ReSharper disable once DoNotCallOverridableMethodsInConstructor
					ParallelOperationThreadCountName, null);
			var serverTimeOutMinutes =
				// ReSharper disable once DoNotCallOverridableMethodsInConstructor
				ConfigurationManager.AppSettings.GetValue<int?>(ServerTimeOutName, null);
			if (serverTimeOutMinutes.HasValue)
			{
				_serverTimeOut = new TimeSpan(0, serverTimeOutMinutes.GetValueOrDefault(), 0);
			}
			_singleUploadThresholdInBytes =
				ConfigurationManager.AppSettings.GetValue<long?>(
					// ReSharper disable once DoNotCallOverridableMethodsInConstructor
					SingleUploadThresholdInBytesName, null);
		}

		/// <summary>
		/// Gets the name of the parallel operation thread count.
		/// </summary>
		/// <value>The name of the parallel operation thread count.</value>
		protected abstract string ParallelOperationThreadCountName { get; }

		/// <summary>
		/// Gets the name of the server time out.
		/// </summary>
		/// <value>The name of the server time out.</value>
		protected abstract string ServerTimeOutName { get; }

		/// <summary>
		/// Gets the name of the single upload threshold in bytes.
		/// </summary>
		/// <value>The name of the single upload threshold in bytes.</value>
		protected abstract string SingleUploadThresholdInBytesName { get; }

		/// <summary>
		/// Gets the parallel operation thread count.
		/// </summary>
		/// <value>The parallel operation thread count.</value>
		public int? ParallelOperationThreadCount
		{
			get { return _parallelOperationThreadCount; }
		}

		/// <summary>
		/// Gets the server time out.
		/// </summary>
		/// <value>The server time out.</value>
		public TimeSpan? ServerTimeOut
		{
			get { return _serverTimeOut; }
		}

		/// <summary>
		/// Gets the single upload threshold in bytes.
		/// </summary>
		/// <value>The single upload threshold in bytes.</value>
		public long? SingleUploadThresholdInBytes
		{
			get { return _singleUploadThresholdInBytes; }
		}
	}
}