using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// class that delegate work to the Terms and Conditions Repository
    /// </summary>
    public class TermsAndConditionsProvider : ITermsAndConditionsProvider
    {
        private readonly ITermsAndConditionsRepository _termsAndConditionsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsProvider"/> class.
        /// </summary>
        /// <param name="termsAndConditionsRepository">The terms and conditions repository.</param>
        public TermsAndConditionsProvider(ITermsAndConditionsRepository termsAndConditionsRepository)
        {
            _termsAndConditionsRepository = termsAndConditionsRepository;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="conditionsType">Type of the conditions.</param>
        /// <returns></returns>
        public IEnumerable<TermsAndConditions> FindByType(TermsAndConditionsType conditionsType)
        {
            return _termsAndConditionsRepository.FindAll().Where(i => i.Type == conditionsType);
        }

        /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="id">The GUID.</param>
        /// <returns></returns>
        public TermsAndConditions FetchById(Guid id)
        {
            return _termsAndConditionsRepository.FindById(id);
        }
    }
}
