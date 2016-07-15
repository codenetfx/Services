using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Performance.Configuration
{
	/// <summary>
	/// Provides an implementation for the IPerformanceConfigurationSource interface.
	/// </summary>
	public class PerformanceConfigurationSource : IPerformanceConfigurationSource
	{
		private string[] _args = {};
		private int _iterations;
		private string _logFilename;
		private const int DefaultIterations = 1;
		private readonly string _defaultLogFileName;

		/// <summary>
		/// Initializes a new instance of the <see cref="PerformanceConfigurationSource" /> class.
		/// </summary>
		public PerformanceConfigurationSource()
		{
			_defaultLogFileName = Path.GetTempPath() + @"\UL_Aria_Service_Performance.log";
		}


		/// <summary>
		/// Updates the with runtime arguments.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public void UpdateWithRuntimeArguments(string[] args)
		{
			_args = args;
			var arguments = new Arguments(args.ToList());
			EntityType = arguments.EntityType;
			LogFilename = arguments.LogFile;
			Iterations = arguments.Iterations;
		}

		/// <summary>
		/// Gets an integer representing the iterations.
		/// </summary>
		/// <value>
		/// The iterations.
		/// </value>
		public int Iterations
		{
			get
			{
				if (_iterations == 0)
				{
					int temp;
					int.TryParse(ConfigurationManager.AppSettings["Iterations"], out temp);
					_iterations = (temp > 0) ? temp : DefaultIterations;
				}

				return _iterations;
			}
			private set { _iterations = value; }
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
				if (string.IsNullOrEmpty(_logFilename))
				{
					_logFilename = ConfigurationManager.AppSettings["LogFileName"];

					if (string.IsNullOrEmpty(_logFilename))
						_logFilename = _defaultLogFileName;
				}

				return _logFilename;
			}
			private set { _logFilename = value; }
		}

		/// <summary>
		/// Gets the type of the entity that will be processed.
		/// </summary>
		/// <value>
		/// The type of the entity.
		/// </value>
		public EntityTypeEnumDto EntityType { get; private set; }


		/// <summary>
		/// Provides a class for command line argument parsing specific to the performance application.
		/// </summary>
		[ExcludeFromCodeCoverage]
		private class Arguments
		{
			private readonly string _entityType;
			private readonly string _iterations;

			/// <summary>
			/// Initializes a new instance of the <see cref="Arguments"/> class.
			/// </summary>
			/// <param name="args">The arguments.</param>
			public Arguments(List<string> args)
			{
				LogFile = ResolveArg(args, x => x.ToLowerInvariant() == "-f");
				_iterations = ResolveArg(args, x => x.ToLowerInvariant() == "-i");
				_entityType = ResolveArg(args, x => x.ToLowerInvariant() == "-e");
				if (string.IsNullOrWhiteSpace(_entityType))
				{
					_entityType = "Project";
				}
			}

			/// <summary>
			/// Resolves the argument.
			/// </summary>
			/// <param name="args">The arguments.</param>
			/// <param name="lambda">The lambda.</param>
			/// <returns></returns>
			private static string ResolveArg(List<string> args, Predicate<string> lambda)
			{
				var index = args.FindIndex(lambda);
				if (index >= 0 && index + 1 < args.Count)
				{
					var temp = args[index + 1];
					if (!temp.Trim().StartsWith("-"))
						return temp;
				}

				return string.Empty;
			}

			/// <summary>
			/// Gets the iterations.
			/// </summary>
			/// <value>
			/// The iterations.
			/// </value>
			public int Iterations
			{
				get
				{
					int temp;
					int.TryParse(_iterations, out temp);
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
					EntityTypeEnumDto temp;
					Enum.TryParse(_entityType, out temp);
					return temp;
				}
			}

			/// <summary>
			/// Gets or sets the filename log file.
			/// </summary>
			/// <value>
			/// The log file.
			/// </value>
			public string LogFile { get; private set; }
		}


		/// <summary>
		/// Checks if help was requested, then displays the help info via Console.
		/// </summary>
		/// <returns></returns>
		public bool CheckHelpRequested()
		{
			var args = _args;
			if (args == null || args.Length == 0
			    || args.Any(x => x.ToLowerInvariant() == "-h" || x.ToLowerInvariant() == "-help"))
			{
				Console.WriteLine(
					"-e\tEntity Name to process| supported values: {\"Project\", \"Task\"}");
				Console.WriteLine("-f\tLog file full name e.g. path+filename.log");
				Console.WriteLine("-i\tIterations to perform.");
				return true;
			}

			return false;
		}
	}
}