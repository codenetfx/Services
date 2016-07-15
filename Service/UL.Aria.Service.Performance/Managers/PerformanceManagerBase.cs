using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Performance.Configuration;
using UL.Aria.Service.Performance.Results;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Performance.Managers
{
	/// <summary>
	/// Provides the base implementation for UpdateManager classes.
	/// </summary>
	public abstract class PerformanceManagerBase : Disposable, IPerformanceManager
	{
		private readonly IFileLogger _fileLogger;
		private readonly IPerformanceConfigurationSource _configSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="PerformanceManagerBase" /> class.
		/// </summary>
		/// <param name="fileLogger">The file logger.</param>
		/// <param name="config">The configuration.</param>
		protected PerformanceManagerBase(IFileLogger fileLogger, IPerformanceConfigurationSource config)
		{
			_fileLogger = fileLogger;
			_configSource = config;
		}

		/// <summary>
		/// Runs the update process.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <param name="progressAction"></param>
		public void Run(CancellationToken cancellationToken, Action<ProgressInfo> progressAction)
		{
			try
			{
				Status = ProcessStatus.Processing;
				var lookups = ResolveIncompleteItems();
				var totalItems = lookups.Count();
				var processedCount = 0;


				foreach (var x in lookups)
				{
					var func = GetItemProcessingFunction(x);
					var result = func();

					Interlocked.Increment(ref processedCount);
					progressAction(new ProgressInfo
					{
						TotalItems = totalItems,
						ProcessedCount = processedCount,
						CompletedItemId = result.Lookup.Id.Value
					});

					LogResult(result);
					progressAction(new ProgressInfo
					{
						TotalItems = totalItems,
						ProcessedCount = processedCount
					});
				}
				Status = ProcessStatus.Completed;
			}

			catch (OperationCanceledException ex)
			{
				StatusMessage = ex.Message;
				Status = ProcessStatus.Canceled;
			}
			catch (Exception ex)
			{
				StatusMessage = ex.Message;
				Status = ProcessStatus.Interrupted;
			}
		}


		/// <summary>
		/// Logs the result.
		/// </summary>
		/// <param name="taskExecutionResult">The task execution result.</param>
		protected internal void LogResult(TaskExecutionResult taskExecutionResult)
		{
			_fileLogger.Write(taskExecutionResult.ToString());
		}


		/// <summary>
		/// When implemented in a derived class it gets the Item Processing function.
		/// </summary>
		/// <param name="lookup">The lookup.</param>
		/// <returns></returns>
		protected internal abstract Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup);

		/// <summary>
		/// When implemented in a derived class it resolves a list of items that were not completed
		/// during a previous execution of the process.
		/// </summary>
		/// <returns></returns>
		protected internal abstract IEnumerable<Lookup> ResolveIncompleteItems();

		/// <summary>
		/// Gets a reference to the file logger.
		/// </summary>
		/// <value>
		/// The file logger.
		/// </value>
		protected IFileLogger FileLogger
		{
			get { return _fileLogger; }
		}

		/// <summary>
		/// Gets a reference to the configuration source.
		/// </summary>
		/// <value>
		/// The configuration source.
		/// </value>
		protected IPerformanceConfigurationSource ConfigSource
		{
			get { return _configSource; }
		}

		/// <summary>
		/// Gets a value indicating the Managers process status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		public ProcessStatus Status { get; protected set; }

		/// <summary>
		/// Gets the status message.
		/// </summary>
		/// <value>
		/// The status message.
		/// </value>
		public string StatusMessage { get; protected set; }


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			using (_fileLogger)
			{
			}
		}
	}
}