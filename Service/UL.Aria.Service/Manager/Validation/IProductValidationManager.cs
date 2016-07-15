using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Defines operations to Validates <see cref="Product" /> objects.
    /// </summary>
    public interface IProductValidationManager
    {
        /// <summary>
        ///     Validates the specified entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        IList<string> Validate(Product entityToValidate);
    }
}