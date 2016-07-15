using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;
using System.Collections.Generic;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Repository for <see cref="LocationCode"/>
    /// </summary>
    public interface ILocationCodeRepository : IPrimaryEntityRepository<LocationCode>
    {
		/// <summary>
		/// Finds the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCode.</returns>
	    LocationCode FindByExternalId(string externalId);


        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LocationCode> FetchAll();

    }
}
