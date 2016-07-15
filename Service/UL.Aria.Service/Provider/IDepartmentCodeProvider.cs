using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Defines Operations for DepartmentCode
	/// </summary>
	public interface IDepartmentCodeProvider : ISearchProviderBase<DepartmentCode>
	{
		/// <summary>
		///     Gets all.
		/// </summary>
		/// <returns></returns>
		IEnumerable<DepartmentCode> FetchAll();

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCode.</returns>
		DepartmentCode FetchByExternalId(string externalId);
	}
}