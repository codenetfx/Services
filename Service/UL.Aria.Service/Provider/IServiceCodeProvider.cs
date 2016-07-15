using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines Operations for ServiceCode
    /// </summary>
    public interface IServiceCodeProvider:ISearchProviderBase<ServiceCode>
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ServiceCode> FetchAll();

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCode.</returns>
	    ServiceCode FetchByExternalId(string externalId);
   
    }    
}