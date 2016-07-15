using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Interface IProductFamilyFeatureValueProvider
    /// </summary>
    public interface IProductFamilyFeatureValueProvider
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        IList<ProductFamilyFeatureValue> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        ProductFamilyFeatureValue Fetch(Guid id);

        /// <summary>
        ///     Fetches the by product family id.
        /// </summary>
        /// <param name="productFeatureId">The product feature id.</param>
        /// <returns>IList{ProductFamilyFeatureValue}.</returns>
        IList<ProductFamilyFeatureValue> FetchByProductFeatureId(Guid productFeatureId);

        /// <summary>
        ///     Creates the specified product family feature value.
        /// </summary>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>Guid.</returns>
        Guid Create(ProductFamilyFeatureValue productFamilyFeatureValue);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        void Update(Guid id, ProductFamilyFeatureValue productFamilyFeatureValue);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(Guid id);
    }
}