using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Interface IDocumentTemplateProvider
	/// </summary>
	public interface IDocumentTemplateProvider : ISearchProviderBase<DocumentTemplate>
	{
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
	    IEnumerable<DocumentTemplate> FetchAll();

        /// <summary>
        /// Fetches the document templates by entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
	    IEnumerable<DocumentTemplate> FetchDocumentTemplatesByEntity(Guid entityId);

        /// <summary>
        /// Updates the document template associations.
        /// </summary>
        /// <param name="documentTemplates">The document templates.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateDocumentTemplateAssociations(IEnumerable<DocumentTemplate> documentTemplates, Guid parentId);
    }
}