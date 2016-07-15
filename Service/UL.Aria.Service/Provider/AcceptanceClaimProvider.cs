using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     concrete acceptance claim provider
    /// </summary>
    public class AcceptanceClaimProvider : IAcceptanceClaimProvider
    {
        private readonly IAcceptanceClaimRepository _acceptanceClaimRepository;
        private readonly ITermsAndConditionsRepository _termsAndConditionsRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AcceptanceClaimProvider" /> class.
        /// </summary>
        /// <param name="acceptanceClaimRepository">The acceptance claim repository.</param>
        /// <param name="termsAndConditionsRepository"></param>
        public AcceptanceClaimProvider(IAcceptanceClaimRepository acceptanceClaimRepository,
                                       ITermsAndConditionsRepository termsAndConditionsRepository)
        {
            _acceptanceClaimRepository = acceptanceClaimRepository;
            _termsAndConditionsRepository = termsAndConditionsRepository;
        }

        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IEnumerable<TermsAndConditions> GetTermsAndConditions(Guid userId)
        {
           return _termsAndConditionsRepository.FindByUserId(userId);
        }

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditions">The terms and conditions.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        public void AcceptTermsAndConditions(TermsAndConditions termsAndConditions, Guid userId, bool hasAccepted)
        {
            var acceptanceClaim = new AcceptanceClaim(Guid.NewGuid())
                {
                    Accepted = hasAccepted,
                    TermsAndConditionsId = termsAndConditions.Id.Value,
                    UserId = userId
                };
            _acceptanceClaimRepository.Add(acceptanceClaim);
        }
    }
}