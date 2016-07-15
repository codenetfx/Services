using System.Threading;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Domain.Entity;

using Task = System.Threading.Tasks.Task;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Defines operations for importing <see cref="Product" /> entities from <see cref="ProductUpload" /> objects.
    /// </summary>
    public interface IProductUploadImportManager : IProcessingManager
    {
        /// <summary>
        ///     Imports the specified product upload.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        void Import(ProductUpload productUpload);
    }
}