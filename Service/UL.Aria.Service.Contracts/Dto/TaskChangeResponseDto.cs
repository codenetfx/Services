using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;


namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Server Response Dto for Deletion of a set of Tasks
    /// </summary>
    [DataContract]
    public class TaskChangeResponseDto
    {
        /// <summary>
        /// Gets or sets the deleted entity ids list.
        /// </summary>
        /// <value>
        /// The deleted entity i ds.
        /// </value>
        [DataMember]
        public List<Guid> DeletedEntityIDs { get; set; }

        /// <summary>
        /// Gets or sets the a list of tasks that were updated as a result of a delete request.
        /// </summary>
        /// <value>
        /// The updated tasks.
        /// </value>
        [DataMember]
        public TaskSearchResultSetDto UpdatedTasksSearchResult { get; set; }

    }
}