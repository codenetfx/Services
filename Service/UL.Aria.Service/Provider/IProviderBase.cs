using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines a generic provider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProviderBase<T>
        where T : AuditableEntity, IAuditableEntity, ISearchResult, new()
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
        void Create(T entity);

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
    }
}