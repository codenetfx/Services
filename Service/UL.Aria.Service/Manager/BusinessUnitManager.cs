using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements manager operations for <see cref="BusinessUnit" /> entities
    /// </summary>
    public class BusinessUnitManager : SearchManagerBase<BusinessUnit>, IBusinessUnitManager
    {
        private readonly IBusinessUnitProvider _provider;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessUnitManager" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public BusinessUnitManager(IBusinessUnitProvider provider, ITransactionFactory transactionFactory)
            : base(provider)
        {
            _provider = provider;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public override void Delete(Guid id)
        {
            using (var transaction = _transactionFactory.Create())
            {
                var existing = _provider.Fetch(id);
                if (existing.IsDeletePrevented)
                    throw new InvalidOperationException("This Business Unit may not be deleted.");
                base.Delete(id);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusinessUnit> FetchAll()
        {
            return this._provider.FetchAll();
        }

        /// <summary>
        /// When overridden in derived classes, validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// A list of validations violations. If empty, successful. Should not return null.
        /// </returns>
        protected internal override IEnumerable<ValidationViolationDto> ValidateForSave(BusinessUnit entity)
        {
            var validationViolations = base.ValidateForSave(entity).ToList();
            ValidateBusinessUnitName(entity, validationViolations);
            return validationViolations;
        }

        private void ValidateBusinessUnitName(BusinessUnit entity, List<ValidationViolationDto> violations)
        {
            var searchCriteria = new SearchCriteria {IncludeDeletedRecords = true, Keyword = entity.Name};
            var results = _provider.Search(searchCriteria);
            if (results.Results.Any(x => x.Id != entity.Id && x.Name == entity.Name))
            {
                violations.Add(new ValidationViolationDto
                {
                    Code=ValidationCodes.BusinessUnit.BusinessUnitNameAlreadyExists,
                    Message = string.Format("The name {0} already exists.", entity.Name),
                    Level = ValidationLevelEnumDto.Error
                });
            }
        }
        
    }
}