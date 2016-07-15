using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// interface that defines contracts for managing the acceptaance of claims regarding Terms and Conditiond
    /// </summary>
    public interface IAcceptanceClaimManager
    {
        /// <summary>
        /// Gets the terms and conditions.
        /// </summary>
        /// <param name="userId">The GUID.</param>
        IEnumerable<TermsAndConditions> GetTermsAndConditions(Guid userId);

        /// <summary>
        /// Accepts the terms and conditions.
        /// </summary>
        /// <param name="termsAndConditions">The terms and conditions.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="hasAccepted">if set to <c>true</c> [has accepted].</param>
        void AcceptTermsAndConditions(TermsAndConditions termsAndConditions, Guid userId, bool hasAccepted);
    }
}
