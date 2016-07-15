using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Update.Configuration
{
    /// <summary>
    /// Provides an interface for the Update console application configuation.
    /// </summary>
    public interface IUpdateConfigurationSource
    {
        /// <summary>
        /// Gets the maximum concurrent threads.
        /// </summary>
        /// <value>
        /// The maximum concurrent threads.
        /// </value>
        int MaxConcurrentThreads { get; }

        /// <summary>
        /// Gets the log filename.
        /// </summary>
        /// <value>
        /// The log filename.
        /// </value>
        string LogFilename { get; }

        /// <summary>
        /// Gets the number of re runs.
        /// </summary>
        /// <value>
        /// The number of re runs.
        /// </value>
        int NumberOfReRuns { get; }

        /// <summary>
        /// Gets the item limit.
        /// </summary>
        /// <value>
        /// The item limit.
        /// </value>
        int ItemLimit { get; }

        /// <summary>
        /// Gets the item limit increment.
        /// </summary>
        /// <value>
        /// The item limit increment.
        /// </value>
        int ItemLimitIncrement{get;}

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        EntityTypeEnumDto EntityType { get; }

                /// <summary>
        /// Updates the with runtime arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void UpdateWithRuntimeArguments(string[] args);

        /// <summary>
        /// Checks if help was requested, then displays the help info via Console.
        /// </summary>
        /// <returns></returns>
        bool CheckHelpRequested();
    }
}
