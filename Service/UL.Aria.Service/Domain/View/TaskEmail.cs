using System;

namespace UL.Aria.Service.Domain.View
{
	/// <summary>
	///     Task assignment cahnge emails
	/// </summary>
	public class TaskEmail
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
		///     Gets or sets the end date.
		/// </summary>
		/// <value>
		///     The end date.
		/// </value>
		public DateTime? EndDate { get; set; }

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
	}
}