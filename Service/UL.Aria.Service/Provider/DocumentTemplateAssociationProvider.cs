using System;
using System.Linq;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Provides a Provider implemenation for Document Template Associations.
    /// </summary>
	public class DocumentTemplateAssociationProvider : IDocumentTemplateAssociationProvider
	{
		private readonly IDocumentTemplateAssociationRepository _documentTemplateAssociationRepository;
	    private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTemplateAssociationProvider"/> class.
        /// </summary>
        /// <param name="documentTemplateAssociationRepository">The document template association repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public DocumentTemplateAssociationProvider(IDocumentTemplateAssociationRepository documentTemplateAssociationRepository, IPrincipalResolver principalResolver)
	    {
            _documentTemplateAssociationRepository = documentTemplateAssociationRepository;
		    _principalResolver = principalResolver;
	    }

        /// <summary>
        /// Saves the specified document template associations.
        /// </summary>
        /// <param name="documentTemplateAssociations">The document template associations.</param>
        /// <param name="parentId">The parent identifier.</param>
		public void Save(IEnumerable<DocumentTemplateAssociation> documentTemplateAssociations, Guid parentId)
		{
            documentTemplateAssociations.ToList().ForEach(x => SetupDocumentTemplateAssociation(_principalResolver, x));
			_documentTemplateAssociationRepository.Save(documentTemplateAssociations, parentId);
		}

        /// <summary>
        /// Setups the document template association.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="documentTemplateAssociation">The document template association.</param>
        internal static void SetupDocumentTemplateAssociation(IPrincipalResolver principalResolver, DocumentTemplateAssociation documentTemplateAssociation)
		{
			var currentDateTime = DateTime.UtcNow;
            documentTemplateAssociation.CreatedById = principalResolver.UserId;
            documentTemplateAssociation.CreatedDateTime = currentDateTime;
            documentTemplateAssociation.UpdatedById = principalResolver.UserId;
            documentTemplateAssociation.UpdatedDateTime = currentDateTime;
		}
	}
}
