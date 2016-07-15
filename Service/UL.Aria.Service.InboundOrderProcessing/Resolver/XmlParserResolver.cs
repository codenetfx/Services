using Microsoft.Practices.Unity;

using UL.Aria.Service.Parser;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Class XmlParserResolver. This class cannot be inherited.
    /// </summary>
    public sealed class XmlParserResolver : IXmlParserResolver
    {
        private readonly IUnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlParserResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public XmlParserResolver(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IXmlParser.</returns>
        public IXmlParser Resolve(string name)
        {
            return _container.Resolve<IXmlParser>(name);
        }
    }
}