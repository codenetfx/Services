using System;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Data
{
    /// <summary>
    /// Claim repository interface.
    /// </summary>
    public interface IClaimDefinitionRepository : IRepositoryBase<ClaimDefinition>
    {
        /// <summary>
        /// Finds the by claim id.
        /// </summary>
        /// <param name="claimId">The claim id.</param>
        /// <returns></returns>
        ClaimDefinition FindByClaimId(Uri claimId);
    }
}