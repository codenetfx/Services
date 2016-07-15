using System;

namespace UL.Aria.Service.Domain.View
{
	/// <summary>
	///     Task assignment cahnge emails
	/// </summary>
	public class TaskCompletedEmail
	{
		/// <summary>
		///     Gets or sets the task id.
		/// </summary>
		/// <value>The task id.</value>
		public Guid TaskId { get; set; }

		/// <summary>
		/// Gets or sets the project unique identifier.
		/// </summary>
		/// <value>
		/// The project unique identifier.
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
		///     Gets or sets the name of the task.
		/// </summary>
		/// <value>
		///     The name of the task.
		/// </value>
		public string TaskName { get; set; }

		/// <summary>
		///     Gets or sets the due date.
		/// </summary>
		/// <value>
		///     The due date.
		/// </value>
		public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the previous task owner.
		/// </summary>
		/// <value>
		/// The previous task owner.
		/// </value>
		public string ActorName { get; set; }

		/// <summary>
		/// Gets or sets the actor login unique identifier.
		/// </summary>
		/// <value>
		/// The actor login unique identifier.
		/// </value>
		public string ActorLoginId { get; set; }

		/// <summary>
		/// Gets or sets the name of the recipient.
		/// </summary>
		/// <value>
		/// The name of the recipient.
		/// </value>
		public string RecipientName { get; set; }

        /// <summary>
        /// Gets or sets the task phase.
        /// </summary>
        /// <value>
        /// The task phase.
        /// </value>
        public string TaskPhase { get; set; }

        /// <summary>
        /// Gets or sets the task progress.
        /// </summary>
        /// <value>
        /// The task progress.
        /// </value>
        public string TaskProgress { get; set; }


        /// <summary>
        /// Gets or sets the last posted comment.
        /// </summary>
        /// <value>
        /// The last posted comment.
        /// </value>
	    public string LastPostedComment { get; set; }
	}
}