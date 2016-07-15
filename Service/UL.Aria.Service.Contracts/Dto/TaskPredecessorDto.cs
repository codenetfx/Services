using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class TaskPredecessorDto
    /// </summary>
    [DataContract]
    public class TaskPredecessorDto
    {
        /// <summary>
        ///     Gets or sets the task id.
        /// </summary>
        /// <value>The task id.</value>
        [DataMember]
        public Guid TaskId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        [DataMember]
        public int TaskNumber { get; set; }

        /// <summary>
        /// Gets or sets the successor identifier.
        /// </summary>
        /// <value>
        /// The successor identifier.
        /// </value>
        [DataMember]
        public Guid SuccessorId { get; set; }

		/// <summary>
		///     Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[DataMember]
		public TaskStatusEnumDto Status { get; set; }

    }

}