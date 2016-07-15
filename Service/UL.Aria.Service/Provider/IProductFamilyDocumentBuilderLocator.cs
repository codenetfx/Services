namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines a Locator for <see cref="IProductFamilyDocumentBuilder"/>
    /// </summary>
    public interface IProductFamilyDocumentBuilderLocator
    {
        /// <summary>
        /// Resolves the entity.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IProductFamilyDocumentBuilder Resolve(string name);
    }
}