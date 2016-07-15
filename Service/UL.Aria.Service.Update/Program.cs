using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Update.Configuration;
using UL.Aria.Service.Update.Factory;
using UL.Aria.Service.Update.Managers;

namespace UL.Aria.Service.Update
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>

        static void Main(string[] args)
        {
            //to auto attach when running outside of visual studio
            //uncomment line below
            //System.Diagnostics.Debugger.Launch();
            var provider = UnityInstanceProvider.Create();
            var performanceMetrics = new List<PerformanceMetric>();
            BeginProcessing(provider, args, performanceMetrics);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();            
        }

        private static bool AssureLogFileResuse(string filename)
        {
            try
            {               
                if (!File.Exists(filename))
                {
                    return true;
                }

                Console.WriteLine(String.Format("Log File '{0}' already exists.  Do you want to delete it? [Y] [N]", filename));
                var input = Console.ReadKey();
                Console.WriteLine();
                if (input.Key == ConsoleKey.Y)
                {
                    File.Delete(filename);
                    Console.WriteLine(String.Format("File '{0}' deleted.", filename));
                    return true;
                }              
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception occured while deleting file, check to see if file still exists.\r\nException: {0}", ex.Message));      
            }

            return false;
        }





        private static void BeginProcessing(UnityInstanceProvider provider, string[] args, List<PerformanceMetric> metricList)
        {
            var start = DateTime.Now;
            var config = provider.Resolve<IUpdateConfigurationSource>();
            config.UpdateWithRuntimeArguments(args);
            if (config.CheckHelpRequested())
                return;

            var startingWithFreshLog = AssureLogFileResuse(config.LogFilename);
            if (!startingWithFreshLog)
            {
                Console.WriteLine("You are starting with a log file from a previous process run, are you sure you want to continue? [Y] [N]...\r\n");
                if (Console.ReadKey().Key == ConsoleKey.N)
                {
                    return;
                }
            }

            var factory = provider.Resolve<IUpdateManagerFactory>();
            IUpdateManager manager = null;
            var limit = config.ItemLimit;
            for (int i = 0; i < config.NumberOfReRuns; i++)
            {   

                if(config.NumberOfReRuns > 1 && File.Exists(config.LogFilename))
                {
                    if (manager != null)
                    {
                        manager.Dispose();
                    }
                    //auto delete log  
                    File.Delete(config.LogFilename);                      
                }

                try
                {
                    manager = factory.GetManager(config.EntityType);
                }
                catch (ResolutionFailedException)
                {
                    Console.WriteLine("Entity Type not supported!");
                    Console.WriteLine("Existing process...");
                    return;
                }

                var cancelTokenSource = new System.Threading.CancellationTokenSource();
                Console.WriteLine("Starting processing...");
                Console.Write("Processing: ");
                var transactionTimes = new List<double>();
                var totalItems = manager.RunUpdate(cancelTokenSource.Token, GetProgressUpdater(), transactionTimes,  limit);
                Console.WriteLine();
                Console.WriteLine("Procssing Complete");

                var elapsed = DateTime.Now - start;
                Console.WriteLine(string.Format("Elapsed time: {0}:{1}:{2}.{3}",
                    elapsed.Hours, elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds));

                var average = transactionTimes.Sum() / transactionTimes.Count;
                var variance = (((double)1 / (double)transactionTimes.Count) * transactionTimes.Sum(x => (x - average) * (x - average)));
                var sd = Math.Sqrt(variance);

                metricList.Add(new PerformanceMetric
                {
                    Average = average,
                    Variance = variance,
                    SD =sd,
                    totalProcessed = totalItems,
                    ElapsedTime =elapsed,
                  
                });

                limit += config.ItemLimitIncrement;
            }


            PrintPerformanceMetrics(metricList);

           

        }

        private static void PrintPerformanceMetrics(List<PerformanceMetric> metricList)
        {
            Console.WriteLine("Total Items\tAverage Time(s)\tVariance Time(s)\tStandard Deviation Time(s)\tTotal Elapsed Time (min)");
            string itemFormat = "{0}\t\t{1:0.00000000}\t{2:0.00000000}\t\t{3:0.00000000}\t\t\t{4}";
            var sortedList =  metricList.OrderBy(x=> x.totalProcessed).ToList();
            foreach (var metric in sortedList)
            {
                Console.WriteLine(string.Format(itemFormat, metric.totalProcessed, metric.Average, metric.Variance, metric.SD, metric.ElapsedTime.TotalMinutes));
            }



            Console.Write("Enter total number of items estimate (requires int: 100000");
            var estimateSample = "100000"; // Console.ReadLine();
            var n = Convert.ToInt32(estimateSample);
            Console.WriteLine("\r\nExpected Time: " + (n * metricList.Average(x=> x.Average) / 60).ToString() 
                + " minutes : +-" + ((metricList.Average(x=> x.Variance) * n) / 60).ToString() + " Minutes");

        }

        private class PerformanceMetric
        {
            /// <summary>
            /// Gets or sets the sd.
            /// </summary>
            /// <value>
            /// The sd.
            /// </value>
            public double SD { get; set; }
            /// <summary>
            /// Gets or sets the variance.
            /// </summary>
            /// <value>
            /// The variance.
            /// </value>
            public double Variance { get; set; }
            /// <summary>
            /// Gets or sets the average.
            /// </summary>
            /// <value>
            /// The average.
            /// </value>
            public double Average { get; set; }

            /// <summary>
            /// Gets or sets the elapsed time.
            /// </summary>
            /// <value>
            /// The elapsed time.
            /// </value>
            public TimeSpan ElapsedTime { get; set; }

            /// <summary>
            /// Gets or sets the total processed.
            /// </summary>
            /// <value>
            /// The total processed.
            /// </value>
            public int totalProcessed { get; set; }
        }

        private static Action<Results.ProgressInfo> GetProgressUpdater()
        {
            var left = Console.CursorLeft;
            var top = Console.CursorTop;
            object pad_lock = new object();

            return new Action<Results.ProgressInfo>((info) =>
            {
                lock (pad_lock)
                {
                    var countFormat = Microsoft.VisualBasic.Strings.StrDup(info.TotalItems.ToString().Length, "0");
                    var format = "{0:000}%  -  {1:" + countFormat + "} of {2}";
                    Console.SetCursorPosition(left, top);
                    Console.Write(String.Format(format, info.PercentageComplete, info.ProcessedCount, info.TotalItems));
                }

            });
        }



    }
}
