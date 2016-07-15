using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// concrete implementation of the contract that allow one acces to features having accepted the terms and conditions
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
      InstanceContextMode = InstanceContextMode.PerCall)]
    public class TermsAndConditionsService : ITermsAndConditionsService
    {
        private readonly ITermsAndConditionsProvider _termsAndConditionsProvider;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsService" /> class.
        /// </summary>
        /// <param name="termsAndConditionsProvider">The terms and conditions provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public TermsAndConditionsService(ITermsAndConditionsProvider termsAndConditionsProvider, IMapperRegistry mapperRegistry)
        {
            _termsAndConditionsProvider = termsAndConditionsProvider;
            _mapperRegistry = mapperRegistry;
        }


        /// <summary>
        /// Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public TermsAndConditionsDto FetchById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");

            var term = _termsAndConditionsProvider.FetchById(new Guid(id));
            return _mapperRegistry.Map<TermsAndConditionsDto>(term);
        }
        /// <summary>
        /// Fetches the type of all by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IList<TermsAndConditionsDto> FetchAllByType(string type)
        {
            Guard.IsNotNullOrEmpty(type, "type");

            TermsAndConditionsType termsAndConditionsType = (TermsAndConditionsType) Enum.Parse(typeof (TermsAndConditionsType), type);

            return _termsAndConditionsProvider.FindByType(termsAndConditionsType).Select(i => _mapperRegistry.Map<TermsAndConditionsDto>(i)).ToList();
        }
    }
}
