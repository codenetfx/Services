using System;
using System.Collections.Generic;
using System.ServiceModel;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Claim.Domain;
using UL.Aria.Service.Claim.Provider;

namespace UL.Aria.Service.Claim.Implementation
{
    /// <summary>
    /// Claim definition service class.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
    InstanceContextMode = InstanceContextMode.PerCall)]
    public class ClaimDefinitionService : IClaimDefinitionService
    {
        private readonly IClaimDefinitionProvider _claimDefinitionProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimDefinitionService" /> class.
        /// </summary>
        /// <param name="claimDefinitionProvider">The claim provider.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ClaimDefinitionService(IClaimDefinitionProvider claimDefinitionProvider, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _claimDefinitionProvider = claimDefinitionProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Defines the claim.
        /// </summary>
        /// <param name="claimDefinitionDto">The claim definition dto.</param>
        public void DefineClaim(ClaimDefinitionDto claimDefinitionDto)
        {
            var claimDefinition = _mapperRegistry.Map<ClaimDefinition>(claimDefinitionDto);

            using(var transaction = _transactionFactory.Create())
            {
                _claimDefinitionProvider.Add(claimDefinition);

                transaction.Complete();
            }
        }

        /// <summary>
        /// Removes the claim.
        /// </summary>
        /// <param name="claimDefinitionId">The claim id.</param>
        public void RemoveClaim(string claimDefinitionId)
        {
            using(var transaction = _transactionFactory.Create())
            {
                _claimDefinitionProvider.Remove(new Guid(claimDefinitionId));

                transaction.Complete();
            }
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<ClaimDefinitionDto> GetAll()
        {
            using(var transaction = _transactionFactory.Create())
            {
                var result =  _mapperRegistry.Map<List<ClaimDefinitionDto>>(_claimDefinitionProvider.GetAll());

                transaction.Complete();

                return result;
            }
        }
    }
}