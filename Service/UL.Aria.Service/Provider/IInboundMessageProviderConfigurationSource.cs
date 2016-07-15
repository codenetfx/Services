namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IDocumentContentProviderConfigurationSource
	/// </summary>
	public interface IInboundMessageProviderConfigurationSource
	{
		/// <summary>
		/// Gets the new expire days.
		/// </summary>
		/// <value>The new expire days.</value>
		int NewExpireDays { get; }

		/// <summary>
		/// Gets the failed expire days.
		/// </summary>
		/// <value>The failed expire days.</value>
		int FailedExpireDays { get; }

		/// <summary>
		/// Gets the order message expire days.
		/// </summary>
		/// <value>The order message expire days.</value>
		int OrderMessageExpireDays { get; }
	}
}