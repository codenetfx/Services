using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Update.Configuration
{
    /// <summary>
    /// Provides an implemenatation for the IUpdateConfigurationSource interface.
    /// </summary>
    public class UpdateConfigurationSource : IUpdateConfigurationSource
    {

        private string[] _args = new string[] { };
        private int maxConcurrentThreads;
        private string logFilename;
        private const int defaultMaxThreadCount = 7;
        private readonly string defaultLogFileName;
        private const int defaultReRuns = 1;
   

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConfigurationSource" /> class.
        /// </summary>
        public UpdateConfigurationSource()
        {
            defaultLogFileName = System.IO.Path.GetTempPath() + @"\UL_Aria_Entity_Update.log";

        }


        /// <summary>
        /// Updates the with runtime arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void UpdateWithRuntimeArguments(string[] args)
        {
            this._args = args;
            var arguments = new Arguments(args.ToList());
            this.EntityType = arguments.EntityType;
            this.LogFilename = arguments.LogFile;
            this.MaxConcurrentThreads = arguments.MaxThreads;
            this.NumberOfReRuns = arguments.NumberOfReRuns;
            this.ItemLimit = arguments.ItemLimit;
            this.ItemLimitIncrement = arguments.ItemLimitIncrement;
        }

        /// <summary>
        /// Gets an integer representing the maximum concurrent threads allow.
        /// </summary>
        /// <value>
        /// The maximum concurrent threads.
        /// </value>
        public int MaxConcurrentThreads
        {
            get
            {
                if (maxConcurrentThreads == 0)
                {
                    int temp = 0;
                    int.TryParse(ConfigurationManager.AppSettings["LogFileName"], out temp);

                    maxConcurrentThreads = (temp > 0) ? temp : defaultMaxThreadCount;
                }

                return maxConcurrentThreads;

            }
            private set
            {
                maxConcurrentThreads = value;
            }
        }

        /// <summary>
        /// Gets the log filename.
        /// </summary>
        /// <value>
        /// The log filename.
        /// </value>
        public string LogFilename
        {
            get
            {
                if (string.IsNullOrEmpty(logFilename))
                {
                    logFilename = ConfigurationManager.AppSettings["LogFileName"] as string;

                    if (string.IsNullOrEmpty(logFilename))
                        logFilename = defaultLogFileName;
                }

                return logFilename;
            }
            private set
            {
                logFilename = value;
            }
        }

        /// <summary>
        /// Gets the type of the entity that will be processed.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeEnumDto EntityType { get; private set; }


        /// <summary>
        /// Gets the number of re runs.
        /// </summary>
        /// <value>
        /// The number of re runs.
        /// </value>
        public int NumberOfReRuns { get; set; }

        /// <summary>
        /// Gets or sets the item limit.
        /// </summary>
        /// <value>
        /// The item limit.
        /// </value>
        public int ItemLimit { get; set; }

        /// <summary>
        /// Gets or sets the item limit increment.
        /// </summary>
        /// <value>
        /// The item limit increment.
        /// </value>
        public int ItemLimitIncrement { get; set; }

        /// <summary>
        /// Provides a class for command line argument parsing specfic to the update application.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private class Arguments
        {

            private string entityType;
            private string maxThreads;
            private string numberOfReRuns;
            private string itemLimit;
            private string itemLimitIncrement;

            /// <summary>
            /// Initializes a new instance of the <see cref="Arguments"/> class.
            /// </summary>
            /// <param name="args">The arguments.</param>
            public Arguments(List<string> args)
            {
                this.LogFile = ResolveArg(args, x => x.ToLowerInvariant() == "-f");
                this.maxThreads = ResolveArg(args, x => x.ToLowerInvariant() == "-t");
                this.entityType = ResolveArg(args, x => x.ToLowerInvariant() == "-e");
                this.numberOfReRuns = ResolveArg(args, x => x.ToLowerInvariant() == "-r");
                this.itemLimit = ResolveArg(args, x => x.ToLowerInvariant() == "-n");
                this.itemLimitIncrement = ResolveArg(args, x => x.ToLowerInvariant() == "-i");
            }

            /// <summary>
            /// Resolves the argument.
            /// </summary>
            /// <param name="args">The arguments.</param>
            /// <param name="lamda">The lamda.</param>
            /// <returns></returns>
            private string ResolveArg(List<string> args, Predicate<string> lamda)
            {
                var index = args.FindIndex(lamda);
                if (index >= 0 && index + 1 < args.Count)
                {
                    var temp = args[index + 1];
                    if (!temp.Trim().StartsWith("-"))
                        return temp;
                }

                return string.Empty;
            }

            /// <summary>
            /// Gets the maximum threads.
            /// </summary>
            /// <value>
            /// The maximum threads.
            /// </value>
            public int MaxThreads
            {
                get
                {
                    int temp = 0;
                    int.TryParse(maxThreads, out temp);
                    return temp;
                }
            }

            public int NumberOfReRuns
            {
                get
                {
                    int temp = 0;
                    return (int.TryParse(numberOfReRuns, out temp))
                        ? temp
                        : defaultReRuns;
                }
            }

            public int ItemLimit
            {
                get
                {
                    int temp = 0;
                    int.TryParse(itemLimit, out temp);
                    return temp;
                }
            }


            public int ItemLimitIncrement
            {
                get
                {
                    int temp = defaultReRuns;
                    int.TryParse(itemLimitIncrement, out temp);
                    return temp;
                }
            }


            /// <summary>
            /// Gets the type of the entity.
            /// </summary>
            /// <value>
            /// The type of the entity.
            /// </value>
            public EntityTypeEnumDto EntityType
            {
                get
                {
                    EntityTypeEnumDto temp = 0;
                    Enum.TryParse(this.entityType, out temp);
                    return temp;
                }
            }

            /// <summary>
            /// Gets or sets the filename log file.
            /// </summary>
            /// <value>
            /// The log file.
            /// </value>
            public string LogFile { get; set; }
        }




        /// <summary>
        /// Checks if help was requested, then displays the help info via Console.
        /// </summary>
        /// <returns></returns>
        public bool CheckHelpRequested()
        {
            var args = this._args;
            if (args == null || args.Length == 0
                || args.Any(x => x.ToLowerInvariant() == "-h" || x.ToLowerInvariant() == "-help"))
            {
                Console.WriteLine("-e\tEntity Name to process| supported values: {\"Project\", \"ProjectMeta\", \"IncomingOrder\", \"Task\"}");
                Console.WriteLine("-f\tLog file fullname e.g. path+filename.log");
                Console.WriteLine("-t\tMaximum number of concurent threads to use when processing.");
                Console.WriteLine("-r\tThe number of times to repeat the run.");
                Console.WriteLine("-i\tThe amout of items to increment the limit by before each sequential pass of the data.");
                Console.WriteLine("-n\tThe limit to the number of items in the in a run of the data, always 0...limit");
                return true;
            }

            return false;
        }


     
    }
}
