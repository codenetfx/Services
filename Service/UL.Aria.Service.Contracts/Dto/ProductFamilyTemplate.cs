namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Enum representing types of templates that can be created for <see cref="ProductFamilyDto" />
    /// </summary>
    public enum ProductFamilyTemplate
    {
        /// <summary>
        /// A template for defining the family itself.
        /// </summary>
        Template,
        /// <summary>
        /// A template for defining products based on the family.
        /// </summary>
        ProductTemplate
    }
}