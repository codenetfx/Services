using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	///     Class TaskTemplate
	/// </summary>
	public class TaskTemplate : TaskBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTemplate"/> class.
        /// </summary>
        public TaskTemplate()
        {
            SubTasks = new List<TaskTemplate>();
        }

		/// <summary>
		///     Gets or sets the sub tasks.
		/// </summary>
		/// <value>The sub tasks.</value>
		//[XmlArray("SubTasks")]
		//[XmlArrayItem(Type = typeof(TaskTemplate), ElementName = "SubTask")]
		[XmlElement("TaskTemplate")]
		public List<TaskTemplate> SubTasks { get; set; }


		

		/// <summary>
		/// Gets or sets the task type identifier.
		/// </summary>
		/// <value>
		/// The task type identifier.
		/// </value>
		public Guid? TaskTypeId { get; set; }

        ///// <summary>
        ///// Gets or sets the project template identifier.
        ///// </summary>
        ///// <value>
        ///// The project template identifier.
        ///// </value>
        //public Guid ProjectTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the project template identifier.
        /// </summary>
        /// <value>
        /// The project template identifier.
        /// </value>
        public Guid ProjectTemplateId { get; set; }

	}
}