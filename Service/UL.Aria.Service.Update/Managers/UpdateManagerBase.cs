using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// Provides the base implmenation for UpdateManager classes.
    /// </summary>
    public abstract class UpdateManagerBase : Disposable, IUpdateManager
    {

        private readonly IFileLogger _fileLogger;
        private readonly IUpdateConfigurationSource _configSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectUpdateManager" /> class.
        /// </summary>
        /// <param name="fileLogger">The file logger.</param>
        /// <param name="config">The configuration.</param>
        protected UpdateManagerBase(IFileLogger fileLogger, IUpdateConfigurationSource config)
        {
            this._fileLogger = fileLogger;
            this._configSource = config;
        }

        /// <summary>
        /// Runs the update process.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="progressAction"></param>
        /// <param name="transactionTimes"></param>
        /// <param name="itemLimit"></param>
        public int RunUpdate(System.Threading.CancellationToken cancellationToken, Action<Results.ProgressInfo> progressAction, 
            List<double> transactionTimes = null, int? itemLimit = null)
        {

            int totalItems = 0;

            try
            {
                this.Status = ProcessStatus.Processing;
                var lookups = ResolveIncompleteItems();
                var limit = itemLimit.GetValueOrDefault();

                if(limit > 0 && limit < lookups.Count())
                {
                    lookups = lookups.ToList().GetRange(0, limit);
                }

                totalItems = lookups.Count();
                var processedCount = 0;

                var totalStartTime = DateTime.Now;

                var watcher = Parallel.ForEach(lookups, new ParallelOptions()
                 {
                     MaxDegreeOfParallelism = _configSource.MaxConcurrentThreads,
                     CancellationToken = cancellationToken

                 }, x =>
                 {
                     var startTime = DateTime.Now;
                     var func = GetItemProcessingFunction(x);
                     var result = func();

                     Interlocked.Increment(ref processedCount);
                     progressAction(new ProgressInfo()
                     {
                         TotalItems = totalItems,
                         ProcessedCount = processedCount,
                         CompletedItemId = result.Lookup.Id.Value
                     });

                     LogResult(result);
                     progressAction(new ProgressInfo()
                     {
                         TotalItems = totalItems,
                         ProcessedCount = processedCount
                     });
                     var duration = (DateTime.Now - startTime).TotalSeconds;
                     if (transactionTimes != null)
                         transactionTimes.Add(duration);
                 });


                this.Status = ProcessStatus.Completed;
               
            }

            catch (OperationCanceledException ex)
            {
                this.StatusMessage = ex.Message;
                this.Status = ProcessStatus.Cancelled;
            }
            catch (Exception ex)
            {
                this.StatusMessage = ex.Message;
                this.Status = ProcessStatus.Interupted;
            }

            return totalItems;          
        }


        /// <summary>
        /// Logs the result.
        /// </summary>
        /// <param name="taskExecutionResult">The task execution result.</param>
        internal protected void LogResult(TaskExecutionResult taskExecutionResult)
        {
            this._fileLogger.Write(taskExecutionResult.ToString());
        }


        /// <summary>
        /// When implemented in a derived class it gets the Item Processing function.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <returns></returns>
        internal protected abstract Func<TaskExecutionResult> GetItemProcessingFunction(Lookup lookup);

        /// <summary>
        /// When implemented in a derived class it resolves a list of items that were not completed
        /// during a previous execution of the process.
        /// </summary>
        /// <returns></returns>
        internal protected abstract IEnumerable<Lookup> ResolveIncompleteItems();

        /// <summary>
        /// Gets a reference to the file logger.
        /// </summary>
        /// <value>
        /// The file logger.
        /// </value>
        protected IFileLogger FileLogger { get { return _fileLogger; } }

        /// <summary>
        /// Gets a reference to the configuration source.
        /// </summary>
        /// <value>
        /// The configuration source.
        /// </value>
        public IUpdateConfigurationSource ConfigSource { get { return _configSource; } }

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
            try
            {
                using (_fileLogger)
                { }
            }
            catch (ObjectDisposedException)
            {
            }
            
            base.Dispose(disposing);
        }
    }
}
