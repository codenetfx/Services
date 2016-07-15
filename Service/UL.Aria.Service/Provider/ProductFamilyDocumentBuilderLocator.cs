using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Locator for <see cref="IProductFamilyDocumentBuilder"/>
    /// </summary>
    public class ProductFamilyDocumentBuilderLocator : IProductFamilyDocumentBuilderLocator
    {
        // map that contains pairs of interfaces and
        // references to concrete implementations

        /// <summary>
        ///     The _container
        /// </summary>
        private readonly IUnityContainer _container;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyDocumentBuilderLocator" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ProductFamilyDocumentBuilderLocator(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Resolves the entity.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IProductFamilyDocumentBuilder Resolve(string name)
        {
            return _container.Resolve<IProductFamilyDocumentBuilder>(name);
        }
    }
}