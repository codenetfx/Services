using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Provides a classifier that contains information about the change and the list of field level delta.
    /// </summary>
    [DataContract]
    public class TaskDeltaDto
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDeltaDto"/> class.
        /// </summary>
        public TaskDeltaDto()
        {
            MetaDeltaList = new List<MetaDeltaDto>();
        }

        /// <summary>
        /// Gets or sets the meta delta list.
        /// </summary>
        /// <value>
        /// The meta delta list.
        /// </value>
        [DataMember]
        public List<MetaDeltaDto> MetaDeltaList { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        [DataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [DataMember]
        public string Action { get; set; }
    }
}
