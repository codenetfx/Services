using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// contract that defines operations available for manipulating terms and conditions
    /// </summary>
    public interface ITermsAndConditionsRepository
    {
        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        IList<TermsAndConditions> FindAll();

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        TermsAndConditions FindById(Guid entityId);

        /// <summary>
        /// Finds the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        IEnumerable<TermsAndConditions> FindByUserId(Guid userId);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TermsAndConditions entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        int Update(TermsAndConditions entity);

        /// <summary>
        /// Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        void Remove(Guid entityId);
    }
}