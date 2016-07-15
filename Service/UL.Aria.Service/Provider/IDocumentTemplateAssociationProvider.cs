using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Interface IDocumentTemplateAssociationProvider
    /// </summary>
	public interface IDocumentTemplateAssociationProvider
	{
        /// <summary>
        /// Saves the specified document template associations.
        /// </summary>
        /// <param name="documentTemplateAssociations">The document template associations.</param>
        /// <param name="parentId">The parent identifier.</param>
		void Save(IEnumerable<DocumentTemplateAssociation> documentTemplateAssociations, Guid parentId);
	}
}
