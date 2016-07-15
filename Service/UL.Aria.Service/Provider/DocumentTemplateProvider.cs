using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using System.Linq;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class DocumentTemplateProvider.
	/// </summary>
	public class DocumentTemplateProvider : SearchProviderBase<DocumentTemplate>, IDocumentTemplateProvider
	{
		private readonly IBusinessUnitProvider _businessUnitProvider;
        private readonly IDocumentTemplateRepository _documentTemplateRepository;
        private readonly IDocumentTemplateAssociationProvider _documentTemplateAssociationProvider;

		/// <summary>
        /// Initializes a new instance of the <see cref="DocumentTemplateProvider"/> class.
		/// </summary>
		/// <param name="documentTemplateRepository">The document template repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="businessUnitProvider">The business unit provider.</param>
        /// <param name="documentTemplateAssociationProvider">The document template association provider.</param>
		public DocumentTemplateProvider(IDocumentTemplateRepository documentTemplateRepository,
			IPrincipalResolver principalResolver, IBusinessUnitProvider businessUnitProvider, 
            IDocumentTemplateAssociationProvider documentTemplateAssociationProvider)
			: base(documentTemplateRepository, principalResolver)
		{
            _documentTemplateRepository = documentTemplateRepository;
			_businessUnitProvider = businessUnitProvider;
		    _documentTemplateAssociationProvider = documentTemplateAssociationProvider;
		}

		/// <summary>
		/// Creates the specified document template.
		/// </summary>
		/// <param name="documentTemplate">The document template.</param>
		public override void Create(DocumentTemplate documentTemplate)
		{
			base.Create(documentTemplate);
			_businessUnitProvider.UpdateBulk(documentTemplate.BusinessUnits, documentTemplate.Id.GetValueOrDefault());
		}

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="documentTemplate">The document template.</param>
		public override void Update(Guid id, DocumentTemplate documentTemplate)
		{
			base.Update(id, documentTemplate);
			_businessUnitProvider.UpdateBulk(documentTemplate.BusinessUnits, documentTemplate.Id.GetValueOrDefault());
		}

		/// <summary>
		/// Fetches the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns>IndustryCode</returns>
		public override DocumentTemplate Fetch(Guid id)
		{
			var documentTemplate = base.Fetch(id);
			documentTemplate.BusinessUnits = _businessUnitProvider.FetchGroup(id);
			return documentTemplate;
		}
        
        /// <summary>
        /// Fetches the document templates by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public IEnumerable<DocumentTemplate> FetchDocumentTemplatesByEntity(Guid entityId)
        {
            return _documentTemplateRepository.FetchDocumentTemplatesByEntity(entityId);
        }

        /// <summary>
        /// Updates the document template associations.
        /// </summary>
        /// <param name="documentTemplates">The document templates.</param>
        /// <param name="parentId">The parent identifier.</param>
        public void UpdateDocumentTemplateAssociations(IEnumerable<DocumentTemplate> documentTemplates, Guid parentId)
        {
            var associations = documentTemplates.Select(x => new DocumentTemplateAssociation()
            {
                ParentId = parentId,
                DocumentTemplateId = x.Id.GetValueOrDefault()
            }).ToList();

            _documentTemplateAssociationProvider.Save(associations, parentId);
        }

        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
	    public IEnumerable<DocumentTemplate> FetchAll()
	    {
	        return _documentTemplateRepository.FetchAll();
	    }
	}
}