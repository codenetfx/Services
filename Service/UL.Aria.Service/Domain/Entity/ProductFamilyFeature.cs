namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Product family feature.
    /// </summary>
    public sealed class ProductFamilyFeature : ProductFamilyCharacteristicDomainEntity
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [allow changes].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [allow changes]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowChanges { get; set; }

        /// <summary>
        ///     Gets or sets the allowed values.
        /// </summary>
        /// <value>The allowed values.</value>
        public string AllowedValues { get; set; }
    }
}