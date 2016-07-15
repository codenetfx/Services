using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     provider for <see cref="ServiceCode"/> entities.
    /// </summary>
    public class ServiceCodeProvider : SearchProviderBase<ServiceCode>, IServiceCodeProvider
    {
        private readonly IServiceCodeRepository _repository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCodeProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public ServiceCodeProvider(IServiceCodeRepository repository, IPrincipalResolver principalResolver)
            :base(repository, principalResolver)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceCode> FetchAll()
        {
            return _repository.FetchAll();
        }      

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCode.</returns>
		public ServiceCode FetchByExternalId(string externalId)
		{
			return _repository.FindByExternalId(externalId);
		}


    }
}
