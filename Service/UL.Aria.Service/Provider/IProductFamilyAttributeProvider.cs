using System;
using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Interface IProductFamilyAttributeProvider
    /// </summary>
    public interface IProductFamilyAttributeProvider
    {
        /// <summary>
        /// Creates the specified product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        /// <returns>
        /// Guid.
        /// </returns>
        Guid Create(ProductFamilyAttribute productFamilyAttribute);

        /// <summary>
        /// Updates the specified product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        void Update(ProductFamilyAttribute productFamilyAttribute);

        /// <summary>
        ///     Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Characteristic.</returns>
        ProductFamilyAttribute Get(Guid id);

        /// <summary>
        /// Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        IList<ProductFamilyAttribute> GetByProductFamilyId(Guid productFamilyId);

        /// <summary>
        /// Gets the specified product family attribute ids.
        /// </summary>
        /// <param name="productFamilyIds">The product family attribute ids.</param>
        /// <returns></returns>
        IList<ProductFamilyAttribute> Get(IList<Guid> productFamilyIds);

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="characteristicId">The characteristic id.</param>
        /// <param name="option">The option.</param>
        void AddOption(Guid characteristicId, ProductFamilyCharacteristicOption option);

        /// <summary>
        /// Adds the option.
        /// </summary>
        /// <param name="optionId">The option id.</param>
        void RemoveOption(Guid optionId);

        /// <summary>
        /// Finds the by scope id.
        /// </summary>
        /// <param name="scopeId">The scope id.</param>
        /// <returns></returns>
        IEnumerable<ProductFamilyAttribute> FindByScopeId(Guid scopeId);

        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        void Delete(Guid id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyAttribute> GetAll();
    }
}