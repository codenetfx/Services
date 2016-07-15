using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines repository operations for working with <see cref="DocumentTemplateAssociation"/>objects.
    /// </summary>
    public interface IDocumentTemplateAssociationRepository : IAssociationRepository<DocumentTemplateAssociation>
    {
    }
}
