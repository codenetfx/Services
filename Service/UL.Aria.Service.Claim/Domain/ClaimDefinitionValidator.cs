using System.Collections.Generic;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Claim.Domain
{
    /// <summary>
    /// Claim definition validator.
    /// </summary>
    public class ClaimDefinitionValidator : ValidatorBase<ClaimDefinition>
    {
        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="errors">The errors.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void ValidateInstance(ClaimDefinition entityToValidate, List<string> errors)
        {
            if (null == entityToValidate.ClaimId || string.IsNullOrEmpty(entityToValidate.ClaimId.ToString()))
            {
                errors.Add("Missing claim identifier");
            }
        }
    }
}