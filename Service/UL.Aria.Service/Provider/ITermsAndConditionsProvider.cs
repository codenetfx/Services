using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// contract that defines the delegation of to the repository
    /// </summary>
    public interface ITermsAndConditionsProvider
    {
        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="conditionsType">Type of the conditions.</param>
        /// <returns></returns>
        IEnumerable<TermsAndConditions> FindByType(TermsAndConditionsType conditionsType);

        /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="id">The GUID.</param>
        /// <returns></returns>
        TermsAndConditions FetchById(Guid id);
    }
}