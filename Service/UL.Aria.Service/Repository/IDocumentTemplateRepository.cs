using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Interface IDocumentTemplateRepository
    /// </summary>
    public interface IDocumentTemplateRepository : IPrimaryAssocatedRepository<DocumentTemplate>
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
    }
}