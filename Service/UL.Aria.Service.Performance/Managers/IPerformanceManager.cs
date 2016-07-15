using System;
using System.Threading;

using UL.Aria.Service.Performance.Configuration;
using UL.Aria.Service.Performance.Results;

namespace UL.Aria.Service.Performance.Managers
{
	/// <summary>
	/// Interface IPerformanceManager
	/// </summary>
	public interface IPerformanceManager
	{
		/// <summary>
		/// Runs the update process.
		/// </summary>
		void Run(CancellationToken cancellationToken, Action<ProgressInfo> progressAction);


		/// <summary>
		/// Gets a value indicating the Managers process status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		ProcessStatus Status { get; }


		/// <summary>
		/// Gets the status message.
		/// </summary>
		/// <value>
		/// The status message.
		/// </value>
		string StatusMessage { get; }
	}
}