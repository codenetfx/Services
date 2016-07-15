using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Base class for <see cref="Task"/> history.
    /// </summary>
    [Serializable]
    public abstract class TaskHistoryBase
    {
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public abstract string Value { get; }
    }
}