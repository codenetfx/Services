using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// interface decribing operations available for repository
    /// </summary>
    public interface IAcceptanceClaimRepository
    {
        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        IList<AcceptanceClaim> FindAll();

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        AcceptanceClaim FindById(Guid entityId);

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(AcceptanceClaim entity);

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        int Update(AcceptanceClaim entity);

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        void Remove(Guid entityId);
    }
}