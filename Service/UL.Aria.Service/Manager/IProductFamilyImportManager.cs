using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Defines Operations for Importing famlilys.
    /// </summary>
    public interface IProductFamilyImportManager
    {
        /// <summary>
        ///     Imports the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        ProductFamilyUploadResult Import(Stream stream);

        /// <summary>
        ///     Creates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies">The dependencies.</param>
        void Create(ProductFamily family, IEnumerable<ProductFamilyCharacteristicAssociationModel> characteristicUploads, IList<ProductFamilyFeatureDependency> dependencies);

        /// <summary>
        ///     Updates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies">The dependencies.</param>
        void Update(ProductFamily family, IEnumerable<ProductFamilyCharacteristicAssociationModel> characteristicUploads, IList<ProductFamilyFeatureDependency> dependencies);
    }
}