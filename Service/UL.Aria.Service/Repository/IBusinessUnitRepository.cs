using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	///     Defines repository operations for <see cref="BusinessUnit" />
	/// </summary>
	public interface IBusinessUnitRepository : Enterprise.Foundation.Data.IPrimaryAssocatedRepository<BusinessUnit>
	{
		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		IEnumerable<BusinessUnit> FetchAll();

		/// <summary>
		/// Fetches the business unit by entity.
		/// </summary>
		/// <param name="parentId">The parent identifier.</param>
		/// <returns>IEnumerable&lt;BusinessUnit&gt;.</returns>
		IEnumerable<BusinessUnit> FetchBusinessUnitByEntity(Guid parentId);
	}
}