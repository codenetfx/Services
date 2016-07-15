using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// A <see cref="Project"/> with its <see cref="Task"/> objects.
    /// </summary>
    public class ProjectDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectDetail"/> class.
        /// </summary>
        public ProjectDetail()
        {
            Tasks = new List<Task>();
            ProductIds = new List<Guid>();
            ParentTasks = new Dictionary<Guid, Task>();
        }
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public Project Project { get; set; }

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public List<Task> Tasks { get; private set; }

        /// <summary>
        /// Gets the parent tasks.
        /// </summary>
        /// <value>
        /// The parent tasks.
        /// </value>
        public IDictionary<Guid, Task> ParentTasks { get; private set; }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public List<Guid> ProductIds { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can delete tasks.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can delete tasks; otherwise, <c>false</c>.
        /// </value>
        public bool CanDeleteTasks { get; set; }
    }
}