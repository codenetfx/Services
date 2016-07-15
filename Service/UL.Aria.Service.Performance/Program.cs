using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;

using Microsoft.Practices.Unity;
using Microsoft.VisualBasic;

using UL.Aria.Service.Performance.Configuration;
using UL.Aria.Service.Performance.Factory;
using UL.Aria.Service.Performance.Managers;
using UL.Aria.Service.Performance.Results;

namespace UL.Aria.Service.Performance
{
	[ExcludeFromCodeCoverage]
	internal class Program
	{
		private static void Main(string[] args)
		{
			var provider = UnityInstanceProvider.Create();
			BeginProcessing(provider, args);
		}

		private static void BeginProcessing(UnityInstanceProvider provider, string[] args)
		{
			var config = provider.Resolve<IPerformanceConfigurationSource>();
			config.UpdateWithRuntimeArguments(args);
			if (config.CheckHelpRequested())
				return;

			var factory = provider.Resolve<IPerformanceManagerFactory>();
			IPerformanceManager manager;

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

			var cancelTokenSource = new CancellationTokenSource();
			Console.WriteLine("Starting processing...");
			Console.Write("Processing: ");
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			manager.Run(cancelTokenSource.Token, GetProgressUpdater());
			stopWatch.Stop();
			var ts = stopWatch.Elapsed;
			var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
				ts.Hours, ts.Minutes, ts.Seconds,
				ts.Milliseconds / 10);
			Console.WriteLine();
			Console.WriteLine(elapsedTime, "RunTime");
			Console.WriteLine("Processing Complete");
		}

		private static Action<ProgressInfo> GetProgressUpdater()
		{
			var left = Console.CursorLeft;
			var top = Console.CursorTop;
			var padLock = new object();

			return info =>
			{
				lock (padLock)
				{
					var countFormat = Strings.StrDup(info.TotalItems.ToString(CultureInfo.InvariantCulture).Length, "0");
					var format = "{0:000}%  -  {1:" + countFormat + "} of {2}";
					Console.SetCursorPosition(left, top);
					Console.Write(format, info.PercentageComplete, info.ProcessedCount, info.TotalItems);
				}
			};
		}
	}
}