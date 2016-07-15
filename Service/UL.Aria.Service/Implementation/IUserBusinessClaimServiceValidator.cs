using System;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Validates and formats parameters for claims.
    /// </summary>
    public interface IUserBusinessClaimServiceValidator
    {
        /// <summary>
        /// Parses the search parameters.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="loginId">The user id.</param>
        /// <param name="mappedDto">The mapped dto.</param>
        /// <param name="validatedLoginId"></param>
        /// <returns></returns>
        bool TryParseSearchParameters(string claim, string loginId, out BusinessClaim mappedDto, out string validatedLoginId);
    }
}