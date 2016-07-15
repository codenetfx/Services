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
    /// Defines operations for working with <see cref="ProductFamilyAttribute"/> objects.
    /// </summary>
    public interface IProductFamilyAttributeManager
    {
        /// <summary>
        /// Creates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        /// <returns>Product family attribute id.</returns>
        Guid Create(ProductFamilyAttribute productFamilyAttribute);

        /// <summary>
        /// Updates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttribute">The product family attribute.</param>
        void Update(ProductFamilyAttribute productFamilyAttribute);

        /// <summary>
        /// Gets the product family attribute by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Product family attribute data transfer object.</returns>
        ProductFamilyAttribute FetchById(Guid id);

        /// <summary>
        /// Fetches the by unique identifier.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyAttribute> FetchAll();

        /// <summary>
        /// Fetches the by their scope unique identifier.
        /// </summary>
        /// <returns></returns>
        IList<ProductFamilyAttribute> FetchAll(IEnumerable<Guid> scopeIds);

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        void Remove(Guid id);
    }
}
