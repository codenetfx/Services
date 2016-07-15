using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     stub
    /// </summary>
    public class IndustryCodeProvider : SearchProviderBase<IndustryCode>, IIndustryCodeProvider
    {
        private readonly IIndustryCodeRepository _repository;


        /// <summary>
        /// Initializes a new instance of the <see cref="IndustryCodeProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public IndustryCodeProvider(IIndustryCodeRepository repository, IPrincipalResolver principalResolver)
            : base(repository, principalResolver)
        {
            _repository = repository;
        }


		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>IndustryCode.</returns>
		public IndustryCode FetchByExternalId(string externalId)
		{
			return _repository.FindByExternalId(externalId);
		}

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IndustryCode> FetchAll()
        {
            return _repository.FetchAll();
        }

   
    }
}
