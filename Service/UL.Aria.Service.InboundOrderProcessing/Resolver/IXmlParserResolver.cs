using UL.Aria.Service.Parser;

namespace UL.Aria.Service.InboundOrderProcessing.Resolver
{
    /// <summary>
    ///     Interface IXmlParserResolver
    /// </summary>
    public interface IXmlParserResolver
    {
        /// <summary>
        ///     Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IXmlParser.</returns>
        IXmlParser Resolve(string name);
    }
}