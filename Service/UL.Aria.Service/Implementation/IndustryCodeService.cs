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
    ///     Class IndustryCodeService
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false, InstanceContextMode = InstanceContextMode.PerCall)]
    public class IndustryCodeService : IIndustryCodeService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IIndustryCodeProvider _industryCodeProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IndustryCodeService" /> class.
        /// </summary>
        /// <param name="industryCodeProvider">The industry code provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public IndustryCodeService(IIndustryCodeProvider industryCodeProvider, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _industryCodeProvider = industryCodeProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{UnitOfMeasureDto}.</returns>
        public IList<IndustryCodeDto> FetchAll()
        {
            return _industryCodeProvider.FetchAll().Select(_mapperRegistry.Map<IndustryCodeDto>).ToList();
        }

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IndustryCodeDto</returns>
        public IndustryCodeDto Fetch(string id)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");

            var industryCode = _industryCodeProvider.Fetch(id.ToGuid());

            return _mapperRegistry.Map<IndustryCodeDto>(industryCode);
        }

		/// <summary>
		/// Fetches the by external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <returns>IndustryCodeDto.</returns>
		public IndustryCodeDto FetchByExternalId(string externalId)
		{
			Guard.IsNotNullOrEmpty(externalId, "externalId");

			var industryCode = _industryCodeProvider.FetchByExternalId(externalId.DecodeFrom64());

			return _mapperRegistry.Map<IndustryCodeDto>(industryCode);
		}

        /// <summary>
        ///     Creates the specified industry code.
        /// </summary>
        /// <param name="industryCode">The industry code.</param>
        public void Create(IndustryCodeDto industryCode)
        {
            Guard.IsNotNull(industryCode, "IndustryCode");

            var industryCodeBo = _mapperRegistry.Map<IndustryCode>(industryCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _industryCodeProvider.Create(industryCodeBo);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="industryCode">The industry code.</param>
        public void Update(string id, IndustryCodeDto industryCode)
        {
            Guard.IsNotNullOrEmpty(id, "Id");
            var convertedId = id.ToGuid();
            Guard.IsNotEmptyGuid(convertedId, "Id");
            Guard.IsNotNull(industryCode, "IndustryCode");

            var industryCodeBo = _mapperRegistry.Map<IndustryCode>(industryCode);

            using (var transactionScope = _transactionFactory.Create())
            {
                _industryCodeProvider.Update(id.ToGuid(), industryCodeBo);
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
                _industryCodeProvider.Delete(convertedId);
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
            return _mapperRegistry.Map<LookupCodeSearchResultSetDto>(_industryCodeProvider.Search(criteria));

        }
    }
}