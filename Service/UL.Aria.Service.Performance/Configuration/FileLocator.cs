using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Performance.Configuration
{
	/// <summary>
	/// Provides a implementation class to get a filename from the config file using the "FileLogger_LogFileName"
	/// setting name.
	/// </summary>
	public class FileLocator : IFileLocator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileLocator"/> class.
		/// </summary>
		public FileLocator(IPerformanceConfigurationSource config)
		{
			Filename = config.LogFilename;
		}

		/// <summary>
		/// Gets the filename.
		/// </summary>
		/// <value>
		/// The filename.
		/// </value>
		public string Filename { get; set; }
	}
}