using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Attribute for information about how a <see cref="TaskPropertyType" /> should be applied to a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class TaskPropertyTypeAttribute : Attribute
    {
        /// <summary>
        ///     Gets or sets the identifier. Must be parseable to Guid
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }
    }
}