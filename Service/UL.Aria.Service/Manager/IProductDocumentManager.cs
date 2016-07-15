using System;
using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for fetching product documents.
    /// </summary>
    public interface IProductDocumentManager
    {
        /// <summary>
        /// Fetches the specified product document.
        /// </summary>
        /// <param name="product">The product.</param>
        Stream Get (Product product);

        /// <summary>
        /// Gets the document for a group of products
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="familyId"></param>
        /// <returns></returns>
        Stream Get(IEnumerable<Product> products, Guid familyId);
    }
}