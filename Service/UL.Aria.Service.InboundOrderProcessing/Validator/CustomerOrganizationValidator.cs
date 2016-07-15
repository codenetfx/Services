using System.Collections.Generic;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    /// Validator for customer related messages.
    /// </summary>
    public class CustomerOrganizationValidator : IValidator
    {
        /// <summary>
        /// Validates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>
        /// IDictionary{System.StringSystem.String}.
        /// </returns>
        public IDictionary<string, string> Validate(object dto)
        {
            return new Dictionary<string, string>();
        }
    }
}