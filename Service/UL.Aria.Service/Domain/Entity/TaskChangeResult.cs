using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Provides a package for returning task and ids that have beed updated and deleted, 
    /// respectiviely, as a result of a Delete bulk tasks operation
    /// </summary>
    public class TaskChangeResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskChangeResult"/> class.
        /// </summary>
        public TaskChangeResult() 
        {
            UpdatedTasks = new List<Task>();
            this.DeletedTasks = new List<Task>();
        }

        /// <summary>
        /// Gets or sets the update task.
        /// </summary>
        /// <value>
        /// The update task.
        /// </value>
        public List<Task> UpdatedTasks { get; set; }

        /// <summary>
        /// Gets or sets the deleted task ids.
        /// </summary>
        /// <value>
        /// The deleted task ids.
        /// </value>
        public List<Task> DeletedTasks { get; set; }
    }
}
