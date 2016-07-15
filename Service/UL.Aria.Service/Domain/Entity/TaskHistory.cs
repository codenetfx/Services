using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class TaskHistory
    /// </summary>
    public class TaskHistory
    {
        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public Task Task { get; set; }
    }
}