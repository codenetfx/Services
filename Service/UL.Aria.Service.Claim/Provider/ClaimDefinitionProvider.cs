using System;
using System.Collections.Generic;
using UL.Enterprise.Foundation.Domain;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;

namespace UL.Aria.Service.Claim.Provider
{
    /// <summary>
    /// Claim provider class.
    /// </summary>
    public class ClaimDefinitionProvider : IClaimDefinitionProvider
    {
        private readonly IClaimDefinitionRepository _claimDefinitionRepository;
        private readonly IValidator<ClaimDefinition> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimDefinitionProvider" /> class.
        /// </summary>
        /// <param name="claimDefinitionRepository">The claim repository.</param>
        /// <param name="validator">The validator.</param>
        public ClaimDefinitionProvider(IClaimDefinitionRepository claimDefinitionRepository, IValidator<ClaimDefinition> validator)
        {
            _claimDefinitionRepository = claimDefinitionRepository;
            _validator = validator;
        }

        /// <summary>
        /// Defines the claim.
        /// </summary>
        /// <param name="claimDefinition"></param>
        public void Add(ClaimDefinition claimDefinition)
        {
            _validator.AssertIsValid(claimDefinition);

            _claimDefinitionRepository.Add(claimDefinition);
        }

        /// <summary>
        /// Removes the specified claim id.
        /// </summary>
        /// <param name="claimDefinitionId">The claim id.</param>
        public void Remove(Guid claimDefinitionId)
        {
            _claimDefinitionRepository.Remove(claimDefinitionId);
        }

        /// <summary>
        /// Gets all the claim definitions.
        /// </summary>
        /// <returns></returns>
        public IList<ClaimDefinition> GetAll()
        {
            return _claimDefinitionRepository.FindAll();
        }
    }
}