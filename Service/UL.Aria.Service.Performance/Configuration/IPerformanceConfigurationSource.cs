using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Performance.Configuration
{
	/// <summary>
	/// Provides an interface for the Update console application configuration.
	/// </summary>
	public interface IPerformanceConfigurationSource
	{
		/// <summary>
		/// Gets the iterations.
		/// </summary>
		/// <value>
		/// The iterations.
		/// </value>
		int Iterations { get; }

		/// <summary>
		/// Gets the log filename.
		/// </summary>
		/// <value>
		/// The log filename.
		/// </value>
		string LogFilename { get; }

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