using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Mapping betwee project and tasks
    /// </summary>
    public class TaskProjectMapping
    {
        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>
        /// The project id.
        /// </value>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public Task Task { get; set; }
    }
}