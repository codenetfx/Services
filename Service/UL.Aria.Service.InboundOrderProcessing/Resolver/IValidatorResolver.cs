using UL.Aria.Service.InboundOrderProcessing.Validator;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Interface IValidatorResolver
    /// </summary>
    public interface IValidatorResolver
    {
        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IValidator.</returns>
        IValidator Resolve(string name);
    }
}