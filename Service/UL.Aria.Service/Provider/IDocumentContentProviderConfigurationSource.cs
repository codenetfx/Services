using System;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IDocumentContentProviderConfigurationSource
	/// </summary>
	public interface IDocumentContentProviderConfigurationSource
	{
		/// <summary>
		/// Gets the document service URI.
		/// </summary>
		/// <value>The document service URI.</value>
		Uri DocumentServiceUri { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is outbound document enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is outbound document enabled; otherwise, <c>false</c>.</value>
		bool IsOutboundDocumentEnabled { get; }
	}
}