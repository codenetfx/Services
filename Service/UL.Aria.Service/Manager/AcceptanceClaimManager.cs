using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// class that implemnts claim acceptance for Terms and Conditions (NOTE: A Stub for now to not block UI devs)
    /// </summary>
    public class AcceptanceClaimManager : IAcceptanceClaimManager
    {
        private readonly IAcceptanceClaimProvider _acceptanceClaimProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptanceClaimManager"/> class.
        /// </summary>
        /// <param name="acceptanceClaimProvider">The acceptance claim provider.</param>
        public AcceptanceClaimManager(IAcceptanceClaimProvider acceptanceClaimProvider)
        {
            _acceptanceClaimProvider = acceptanceClaimProvider;
        }

        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="userId">The GUID.</param>
        public IEnumerable<TermsAndConditions> GetTermsAndConditions(Guid userId)
        {
            return _acceptanceClaimProvider.GetTermsAndConditions(userId);
        }

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditions">The terms and conditions.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        public void AcceptTermsAndConditions(TermsAndConditions termsAndConditions, Guid userId, bool hasAccepted)
        {
            _acceptanceClaimProvider.AcceptTermsAndConditions(termsAndConditions, userId, hasAccepted);
        }
    }
}