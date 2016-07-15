using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Implements operations for a service for <see cref="BusinessUnitDto"/> entites.
    /// </summary>
    [AutoRegisterRestService]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class BusinessUnitService :IBusinessUnitService
    {
        private readonly IBusinessUnitManager _businessUnitManager;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitService" /> class.
        /// </summary>
        /// <param name="businessUnitManager">The business unit manager.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="mapperRegistry"></param>
        public BusinessUnitService(IBusinessUnitManager businessUnitManager, IAuthorizationManager authorizationManager, IMapperRegistry mapperRegistry)
        {
            _businessUnitManager = businessUnitManager;
            _authorizationManager = authorizationManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        /// <returns></returns>
        public BusinessUnitSearchResultSetDto Search(SearchCriteriaDto searchCriteriaDto)
        {
            Guard.IsNotNull(searchCriteriaDto, "searchCriteria");

            var searchCriteria = _mapperRegistry.Map<SearchCriteria>(searchCriteriaDto);
            var searchResults = _businessUnitManager.Search(searchCriteria);
            
            return _mapperRegistry.Map<BusinessUnitSearchResultSetDto>(searchResults);
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public BusinessUnitDto FetchById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(id, "id");

            var retrieved = _businessUnitManager.Fetch(convertedId);

            return _mapperRegistry.Map<BusinessUnitDto>(retrieved);
        }

        /// <summary>
        /// Creates the specified task template.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BusinessUnitDto Create(BusinessUnitDto entity)
        {
            Guard.IsNotNull(entity, "entity");

            var businessUnit = _mapperRegistry.Map<BusinessUnit>(entity);
            var id = _businessUnitManager.Create(businessUnit);

            var retrieved = _businessUnitManager.Fetch(id);

            return _mapperRegistry.Map<BusinessUnitDto>(retrieved);
        }

        /// <summary>
        /// Validates the specified business unit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<ValidationViolationDto> Validate(BusinessUnitDto entity)
        {
            Guard.IsNotNull(entity, "entity");

            var businessUnit = _mapperRegistry.Map<BusinessUnit>(entity);
            var validations = _businessUnitManager.Validate(businessUnit);

            return validations;
        }

        /// <summary>
        /// Updates the specified business unit identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity">The business unit.</param>
        /// <returns></returns>
        public BusinessUnitDto Update(string id, BusinessUnitDto entity)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(id, "id");
            Guard.IsNotNull(entity, "entity");

            var businessUnit = _mapperRegistry.Map<BusinessUnit>(entity);
            _businessUnitManager.Update(convertedId, businessUnit);

            var retrieved =  _businessUnitManager.Fetch(convertedId);

            return _mapperRegistry.Map<BusinessUnitDto>(retrieved);
        }

        /// <summary>
        /// Deletes the task template by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(id, "id");

            _businessUnitManager.Delete(convertedId);
        }

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusinessUnitDto> FetchAll()
        {
            return _businessUnitManager.FetchAll().Select(x => _mapperRegistry.Map<BusinessUnitDto>(x));
        }
    }
}
