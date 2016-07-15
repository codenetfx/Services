namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface ICryptographyProviderConfigurationSource
	/// </summary>
	public interface ICryptographyProviderConfigurationSource
	{
		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		byte[] Key { get; }

		/// <summary>
		/// Gets the iv.
		/// </summary>
		/// <value>The iv.</value>
		// ReSharper disable once InconsistentNaming
		byte[] IV { get; }
	}
}