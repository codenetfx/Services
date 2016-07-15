using Microsoft.Practices.Unity;

using UL.Aria.Service.InboundOrderProcessing.Validator;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Class ValidatorResolver.
    /// </summary>
    public sealed class ValidatorResolver : IValidatorResolver
    {
        private readonly IUnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidatorResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ValidatorResolver(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IValidator.</returns>
        public IValidator Resolve(string name)
        {
            return _container.Resolve<IValidator>(name);
        }
    }
}