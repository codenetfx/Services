using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Class ServiceCodeService
    /// </summary>
	[AutoRegisterRestServiceAttribute("ServiceCode")]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServiceCodeService : IServiceCodeService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IServiceCodeProvider _serviceCodeProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceCodeService" /> class.
        /// </summary>
        /// <param name="serviceCodeProvider">The Service code provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ServiceCodeService(IServiceCodeProvider serviceCodeProvider, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _serviceCodeProvider = serviceCodeProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{UnitOfMeasureDto}.</returns>
        public IList<ServiceCodeDto> FetchAll()
        {
            return _serviceCodeProvider.FetchAll().Select(_mapperRegistry.Map<ServiceCodeDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ServiceCodeDto</returns>
        public ServiceCodeDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            var serviceCode = _serviceCodeProvider.Fetch(id.ToGuid());

            return _mapperRegistry.Map<ServiceCodeDto>(serviceCode);
        }

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>ServiceCodeDto.</returns>
		public ServiceCodeDto FetchByExternalId(string externalId)
		{
			Guard.IsNotNullOrEmpty(externalId, "externalId");

			var serviceCode = _serviceCodeProvider.FetchByExternalId(externalId.DecodeFrom64());

			return _mapperRegistry.Map<ServiceCodeDto>(serviceCode);
		}

        /// <summary>
        ///     Creates the specified Service code.
        /// </summary>
        /// <param name="serviceCode">The Service code.</param>
        public void Create(ServiceCodeDto serviceCode)
        {
            Guard.IsNotNull(serviceCode, "ServiceCode");

            var serviceCodeBo = _mapperRegistry.Map<ServiceCode>(serviceCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _serviceCodeProvider.Create(serviceCodeBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="serviceCode">The Service code.</param>
        public void Update(string id, ServiceCodeDto serviceCode)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(serviceCode, "ServiceCode");

            var serviceCodeBo = _mapperRegistry.Map<ServiceCode>(serviceCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _serviceCodeProvider.Update(id.ToGuid(), serviceCodeBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            using (var transactionScope = _transactionFactory.Create())
            {
                _serviceCodeProvider.Delete(convertedId);
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public LookupCodeSearchResultSetDto Search(SearchCriteriaDto searchCriteria)
        {
            Guard.IsNotNull(searchCriteria, "searchCriteria");

            var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
            return _mapperRegistry.Map<LookupCodeSearchResultSetDto>(_serviceCodeProvider.Search(criteria));

        }
    }
}