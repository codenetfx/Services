using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Defines a provider for <see cref="BusinessUnit"/> entities.
	/// </summary>
	public interface IBusinessUnitProvider : ISearchProviderBase<BusinessUnit>
	{
		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		IEnumerable<BusinessUnit> FetchAll();

		/// <summary>
		/// Updates the bulk.
		/// </summary>
		/// <param name="businessUnits">The business units.</param>
		/// <param name="parentId">The parent identifier.</param>
		void UpdateBulk(IEnumerable<BusinessUnit> businessUnits, Guid parentId);

		/// <summary>
		/// Fetches the group.
		/// </summary>
		/// <param name="parentId">The parent identifier.</param>
		/// <returns>IEnumerable&lt;BusinessUnit&gt;.</returns>
		IEnumerable<BusinessUnit> FetchGroup(Guid parentId);
	}
}