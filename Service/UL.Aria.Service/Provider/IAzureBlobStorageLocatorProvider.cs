using System;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IAzureBlobStorageLocatorProvider
	/// </summary>
	public interface IAzureBlobStorageLocatorProvider
	{
		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>AzureBlobStorageConfiguration.</returns>
		AzureBlobStorageConfiguration FetchById(Guid? id = null);
	}
}