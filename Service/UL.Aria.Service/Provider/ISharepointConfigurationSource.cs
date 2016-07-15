using System;
using System.Net;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Configuration Options for service proxies.
	/// </summary>
	public interface ISharepointConfigurationSource
	{
		/// <summary>
		///     Gets the task service URL.
		/// </summary>
		/// <value>The task service URL.</value>
		Uri TaskService { get; }

		/// <summary>
		///     Gets the content service.
		/// </summary>
		/// <value>The content service.</value>
		Uri ContentService { get; }

		/// <summary>
		///     Gets the sharepoint search service uri.
		/// </summary>
		/// <value>
		///     The sharepoint search service uri.
		/// </value>
		Uri SearchService { get; }

		/// <summary>
		///     Gets the network credentials.
		/// </summary>
		/// <value>
		///     The network credentials.
		/// </value>
		NetworkCredential Credentials { get; }

		/// <summary>
		/// Gets or sets the time-out value in milliseconds
		/// </summary> 
		/// <returns>
		/// The number of milliseconds to wait before the request times out. The default value is 30,000 milliseconds (30 seconds).
		/// </returns>
		int Timeout { get; }
	}
}