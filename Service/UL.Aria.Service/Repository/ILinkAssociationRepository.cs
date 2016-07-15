using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Interface ILinkAssociationRepository
    /// </summary>
    public interface ILinkAssociationRepository : IRepositoryBase<LinkAssociation>
    {
        /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="linkAssociations">The links.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateBulk(IEnumerable<LinkAssociation> linkAssociations, Guid parentId);

        /// <summary>
        /// Finds the by parent.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        IEnumerable<LinkAssociation> FindByParent(Guid parentId);

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Guid Create(LinkAssociation entity);
    }
}
