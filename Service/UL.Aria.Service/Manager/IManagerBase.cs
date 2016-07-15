using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines a generic manager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IManagerBase<T> where T : class, IAuditableEntity, new()
    {

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IndustryCode</returns>
        T Fetch(Guid id);

        /// <summary>
        ///     Creates the specified industry code.
        /// </summary>
        /// <param name="entity">The industry code.</param>
        Guid Create(T entity);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The unit of measure.</param>
        void Update(Guid id, T entity);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        IEnumerable<ValidationViolationDto> Validate(T entity);
    }
}