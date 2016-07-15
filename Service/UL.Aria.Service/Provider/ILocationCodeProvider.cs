using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Defines Operations for LocationCode
    /// </summary>
    public interface ILocationCodeProvider:ISearchProviderBase<LocationCode>
    {

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>LocationCode.</returns>
        /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
	    LocationCode FetchByExternalId(string externalId);
        
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<LocationCode> FetchAll();     
    }    
}