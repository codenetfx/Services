namespace UL.Aria.Service.Performance.Managers
{
	/// <summary>
	/// Provides a type to indicate the status of a long running process.
	/// </summary>
	public enum ProcessStatus
	{
		/// <summary>
		/// The processing
		/// </summary>
		Processing = 1,

		/// <summary>
		/// The completed
		/// </summary>
		Completed = 2,

		/// <summary>
		/// The interrupted
		/// </summary>
		Interrupted = 3,

		/// <summary>
		/// The canceled
		/// </summary>
		Canceled = 4
	}
}