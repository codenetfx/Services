using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class Sender.
    /// </summary>
    public class Sender
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Int16 Id { get; set; }

        /// <summary>
        ///     Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}