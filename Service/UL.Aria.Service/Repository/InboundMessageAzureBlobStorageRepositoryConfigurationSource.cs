namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class InboundMessageAzureBlobStorageRepositoryConfigurationSource. This class cannot be inherited.
	/// </summary>
	public sealed class InboundMessageAzureBlobStorageRepositoryConfigurationSource : AzureBlobStorageRepositoryConfigurationSourceBase
	{
		/// <summary>
		/// Gets the name of the parallel operation thread count.
		/// </summary>
		/// <value>The name of the parallel operation thread count.</value>
		protected override string ParallelOperationThreadCountName
		{
			get { return "UL.Aria.Service.InboundMessage.ParallelOperationThreadCount"; }
		}

		/// <summary>
		/// Gets the name of the server time out.
		/// </summary>
		/// <value>The name of the server time out.</value>
		protected override string ServerTimeOutName
		{
			get { return "UL.Aria.Service.InboundMessage.ServerTimeOut"; }
		}

		/// <summary>
		/// Gets the name of the single upload threshold in bytes.
		/// </summary>
		/// <value>The name of the single upload threshold in bytes.</value>
		protected override string SingleUploadThresholdInBytesName
		{
			get { return "UL.Aria.Service.InboundMessage.SingleUploadThresholdInBytes"; }
		}
	}
}