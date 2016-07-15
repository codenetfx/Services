using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Provides a classifier that contains information about the change and the list of field level delta.
    /// </summary>
    public class TaskDelta
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDelta"/> class.
        /// </summary>
        public TaskDelta()
        {
            MetaDeltaList = new List<MetaDelta>();
        }

        /// <summary>
        /// Gets or sets the meta delta list.
        /// </summary>
        /// <value>
        /// The meta delta list.
        /// </value>
        [DataMember]
        public List<MetaDelta> MetaDeltaList { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }
    }
}
