using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for Business Unit associations.
    /// </summary>
    public interface IBusinessUnitAssociationRepository : IRepositoryBase<BusinessUnitAssociation>
    {

        /// <summary>
        /// Updates the link associations.
        /// </summary>
        /// <param name="linkAssociations">The links.</param>
        /// <param name="parentId">The parent identifier.</param>
        void UpdateBulk(IEnumerable<BusinessUnitAssociation> linkAssociations, Guid parentId);

        /// <summary>
        /// Finds the by parent.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        IEnumerable<BusinessUnitAssociation> FindByParent(Guid guid);

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Guid Create(BusinessUnitAssociation entity);
    }
}
