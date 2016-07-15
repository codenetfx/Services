using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class TaskContainer.
    /// </summary>
    public class TaskContainer
    {
        /// <summary>
        ///     Gets or sets the container identifier.
        /// </summary>
        /// <value>The container identifier.</value>
        public Guid ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public IEnumerable<Task> Tasks { get; set; }
    }
}