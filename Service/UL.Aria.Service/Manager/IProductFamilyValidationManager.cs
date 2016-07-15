using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Defines operations for Validating a <see cref="ProductFamily" />
    /// </summary>
    public interface IProductFamilyValidationManager
    {
        /// <summary>
        ///     Validates the specified family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="characteristicUploads">The characteristic uploads.</param>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        ProductFamilyUploadResult Validate(ProductFamily family, IEnumerable<ProductFamilyCharacteristicUpload> characteristicUploads, IList<ProductFamilyFeatureAllowedValueDependencyUpload> dependencies);
    }
}