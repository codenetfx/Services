using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines operations for creating product family feature associations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProductFamilyAssociationRepository<T> where T:ProductFamilyCharacteristicAssociation
    {

        /// <summary>
        /// Adds the specified association.
        /// </summary>
        /// <param name="association">The association.</param>
        Guid Create(T association);

        /// <summary>
        /// Gets the by family id.
        /// </summary>
        /// <param name="familyId">The family id.</param>
        /// <returns></returns>
        IEnumerable<T> GetByFamilyId(Guid familyId);

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        void Remove(Guid entityId);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        int Update(T entity);
    }
}
