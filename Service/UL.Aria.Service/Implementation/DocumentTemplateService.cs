using System;
using System.Collections.Generic;
using System.ServiceModel;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// Class DocumentTemplateService.
	/// </summary>
	[AutoRegisterRestService]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class DocumentTemplateService : IDocumentTemplateService
	{
		private readonly IDocumentTemplateManager _documentTemplateManager;
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentTemplateService"/> class.
		/// </summary>
		/// <param name="documentTemplateManager">The document template manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public DocumentTemplateService(IDocumentTemplateManager documentTemplateManager, IMapperRegistry mapperRegistry)
		{
			_documentTemplateManager = documentTemplateManager;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Searches the specified search criteria dto.
		/// </summary>
		/// <param name="searchCriteriaDto">The search criteria dto.</param>
		/// <returns>DocumentTemplateSearchResultSetDto.</returns>
		public DocumentTemplateSearchResultSetDto Search(SearchCriteriaDto searchCriteriaDto)
		{
			Guard.IsNotNull(searchCriteriaDto, "searchCriteria");

			var searchCriteria = _mapperRegistry.Map<SearchCriteria>(searchCriteriaDto);
			var searchResults = _documentTemplateManager.Search(searchCriteria);

			return _mapperRegistry.Map<DocumentTemplateSearchResultSetDto>(searchResults);
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentTemplateDto.</returns>
		public DocumentTemplateDto FetchById(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNull(id, "id");

			var retrieved = _documentTemplateManager.Fetch(convertedId);

			return _mapperRegistry.Map<DocumentTemplateDto>(retrieved);
		}

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>DocumentTemplateDto.</returns>
		public DocumentTemplateDto Create(DocumentTemplateDto entity)
		{
			Guard.IsNotNull(entity, "entity");

			var documentTemplate = _mapperRegistry.Map<DocumentTemplate>(entity);
			var id = _documentTemplateManager.Create(documentTemplate);

			var retrieved = _documentTemplateManager.Fetch(id);

			return _mapperRegistry.Map<DocumentTemplateDto>(retrieved);
		}

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="entity">The entity.</param>
		/// <returns>DocumentTemplateDto.</returns>
		public DocumentTemplateDto Update(string id, DocumentTemplateDto entity)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNull(id, "id");
			Guard.IsNotNull(entity, "entity");

			var documentTemplate = _mapperRegistry.Map<DocumentTemplate>(entity);
			_documentTemplateManager.Update(convertedId, documentTemplate);

			var retrieved = _documentTemplateManager.Fetch(convertedId);

			return _mapperRegistry.Map<DocumentTemplateDto>(retrieved);
		}

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNull(id, "id");

			_documentTemplateManager.Delete(convertedId);
		}

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
	    public IEnumerable<DocumentTemplateDto> FetchAll()
	    {
	        return _mapperRegistry.Map<List<DocumentTemplateDto>>(_documentTemplateManager.FetchAll());
	    }


        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public IEnumerable<ValidationViolationDto> Validate(DocumentTemplateDto entity)
        {
            var documentTemplate = _mapperRegistry.Map<DocumentTemplate>(entity);
            return _documentTemplateManager.Validate(documentTemplate);
        }

	}
}