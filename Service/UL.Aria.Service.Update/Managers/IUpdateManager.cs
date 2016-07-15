using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Results;

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUpdateManager:IDisposable
    {
        /// <summary>
        /// Runs the update process.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="progressAction">The progress action.</param>
        /// <param name="transactionTimes">The transaction times.</param>
        /// <param name="itemLimit">The item limit.</param>
        /// <returns></returns>
        int RunUpdate(System.Threading.CancellationToken cancellationToken, Action<ProgressInfo> progressAction, List<double> transactionTimes = null, int? itemLimit = null);


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


        /// <summary>
        /// Gets the configuration source.
        /// </summary>
        /// <value>
        /// The configuration source.
        /// </value>
        IUpdateConfigurationSource ConfigSource { get; }
    }
}
