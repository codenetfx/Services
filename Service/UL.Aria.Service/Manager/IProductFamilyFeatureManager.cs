using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for working with <see cref="ProductFamilyFeature"/> objects.
    /// </summary>
    public interface IProductFamilyFeatureManager
    {
        /// <summary>
        /// Creates the product family Feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family Feature.</param>
        /// <returns>Product family Feature id.</returns>
        Guid Create(ProductFamilyFeature productFamilyFeature);

        /// <summary>
        /// Updates the product family Feature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family Feature.</param>
        void Update(ProductFamilyFeature productFamilyFeature);

        /// <summary>
        /// Gets the product family Feature by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Product family Feature data transfer object.</returns>
        ProductFamilyFeature FetchById(Guid id);

        /// <summary>
        /// Fetches all features.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyFeature> FetchAll();

        /// <summary>
        /// Fetches the by their scope unique identifier.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyFeature> FetchAll(IEnumerable<Guid> scopeIds);

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        void Remove(Guid id);
    }
}
