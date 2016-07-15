using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines Operations for IndustryCode
    /// </summary>
    public interface IIndustryCodeProvider : ISearchProviderBase<IndustryCode>
    {
        /// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>IndustryCode.</returns>
	    IndustryCode FetchByExternalId(string externalId);

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IndustryCode> FetchAll();
    }
}