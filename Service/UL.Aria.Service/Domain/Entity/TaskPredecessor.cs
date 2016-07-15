using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class TaskPredecessorDto
    /// </summary>
    [Serializable]
    public class TaskPredecessor
    {
        /// <summary>
        ///     Gets or sets the task id.
        /// </summary>
        /// <value>The task id.</value>
        public Guid TaskId { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        public int TaskNumber { get; set; }

        /// <summary>
        /// Gets or sets the successor identifier.
        /// </summary>
        /// <value>
        /// The successor identifier.
        /// </value>
        public Guid SuccessorId { get; set; }

		/// <summary>
		///     Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public Contracts.Dto.TaskStatusEnumDto Status { get; set; }
    }
}