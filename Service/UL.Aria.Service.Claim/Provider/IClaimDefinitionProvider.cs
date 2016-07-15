using System;
using System.Collections.Generic;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Provider
{
    /// <summary>
    /// Claim provider interface.
    /// </summary>
    public interface IClaimDefinitionProvider
    {
        /// <summary>
        /// Defines the claim.
        /// </summary>
        /// <param name="claimDefinition"></param>
        void Add(ClaimDefinition claimDefinition);

        /// <summary>
        /// Removes the specified claim id.
        /// </summary>
        /// <param name="claimDefinitionId">The claim id.</param>
        void Remove(Guid claimDefinitionId);

        /// <summary>
        /// Gets all the claim definitions.
        /// </summary>
        /// <returns></returns>
        IList<ClaimDefinition> GetAll();
    }
}