using System;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Definition of operations for a relay for companies.
    /// </summary>
    public interface IRelayCompanyManager
    {
        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        CompanyDto FetchById(Guid id);

        /// <summary>
        /// Fetches the by external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        CompanyDto FetchByExternalId(string externalId);
    }
}