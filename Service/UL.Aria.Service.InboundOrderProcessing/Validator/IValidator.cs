using System.Collections.Generic;

namespace UL.Aria.Service.InboundOrderProcessing.Validator
{
    /// <summary>
    ///     Interface IValidator
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        ///     Validates the specified dto.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns>IDictionary{System.StringSystem.String}.</returns>
        IDictionary<string, string> Validate(object dto);
    }
}