using System.Collections.Generic;
using System.Xml.Serialization;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class TaskTemplates
    /// </summary>
    public class TaskTemplates
    {
        /// <summary>
        ///     Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        [XmlElement("TaskTemplate")]
        public List<TaskTemplate> Tasks { get; set; }
    }
}