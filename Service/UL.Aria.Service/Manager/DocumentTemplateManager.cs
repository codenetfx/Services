using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Class DocumentTemplateManager.
    /// </summary>
    public class DocumentTemplateManager : SearchManagerBase<DocumentTemplate>, IDocumentTemplateManager
    {
        private readonly IDocumentTemplateProvider _documentTemplateProvider;
        private readonly IBusinessUnitProvider _businessUnitProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTemplateManager" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="businessUnitProvider">The business unit provider.</param>
        public DocumentTemplateManager(IDocumentTemplateProvider provider, IBusinessUnitProvider businessUnitProvider)
            : base(provider)
        {
            _documentTemplateProvider = provider;
            _businessUnitProvider = businessUnitProvider;
        }

        /// <summary>
        /// Validates for save.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected internal override IEnumerable<ValidationViolationDto> ValidateForSave(DocumentTemplate entity)
        {
            var violations = base.ValidateForSave(entity).ToList();

            var allBusinessUnits = _businessUnitProvider.FetchAll();
            var allId = allBusinessUnits.Where(x => x.Code == AssetFieldNames.BusinessUnitAllToken).Select(x => x.Id).FirstOrDefault();
            var searchCriteria = new SearchCriteria { IncludeDeletedRecords = false, Keyword = entity.Name, EndIndex = 99 };
            var results = _documentTemplateProvider.Search(searchCriteria);
            bool matchFound = false;

            if (allId != null && entity.BusinessUnits.Any(x => x.Id == allId))
            {
                matchFound = results.Results.Count(x => x.Id != entity.Id) > 0;
            }
            else
            {
                var entityBusinessUnitCodes = allBusinessUnits
                    .Where(x => entity.BusinessUnits.Select(y => y.Id).Contains(x.Id))
                    .Select(x => x.Code).ToList();

                var activeBu = results.Results
                   .Where(x => x.Id != entity.Id)
                   .SelectMany(x => x.BusinessUnitCodes.Replace(" ", "").Split(','));

                matchFound = activeBu.Contains(AssetFieldNames.BusinessUnitAllToken);

                if (!matchFound)
                    matchFound = activeBu.Intersect(entityBusinessUnitCodes).Any();
            }


            if (matchFound)
            {
                violations.Add(new ValidationViolationDto
                {
                    Code = ValidationCodes.DocumentTemplate.NameBusinessUnitAlreadyExists,
                    Message = "Active record currently exists for this Document Template Name/Business Unit combination(s).",
                    Level = ValidationLevelEnumDto.Error
                });
            }

            return violations;

        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DocumentTemplate> FetchAll()
        {
            return _documentTemplateProvider.FetchAll();
        }
    }
}