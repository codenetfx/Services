namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Unit of Measure
    /// </summary>
    public sealed class UnitOfMeasure : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }
    }
}