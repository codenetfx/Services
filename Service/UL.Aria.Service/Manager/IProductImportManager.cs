using System.Collections.Generic;
using System.IO;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for importing <see cref="Product"/> entities from documents.
    /// </summary>
    public interface IProductImportManager
    {
        /// <summary>
        /// Imports the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        IEnumerable<ProductUploadResult> Import(Stream stream);
    }
}