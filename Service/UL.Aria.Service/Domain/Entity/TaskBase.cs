using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Provides a base structure for Task entities.
    /// </summary>
    [Serializable]
    public abstract class TaskBase : PrimarySearchEntityBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBase"/> class.
        /// </summary>
        protected TaskBase()
        {
            Predecessors = new List<TaskPredecessor>();
        }

        /// <summary>
        ///     Gets or sets the actual duration.
        /// </summary>
        /// <value>The actual duration.</value>
        public double? ActualDuration { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the task owner.
        /// </summary>
        /// <value>The task owner.</value>
        public string TaskOwner { get; set; }

        /// <summary>
        ///     Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        public int TaskNumber { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public Contracts.Dto.TaskStatusEnumDto Status { get; set; }

        /// <summary>
        ///     Gets or sets the predecessors.
        /// </summary>
        /// <value>The predecessors.</value>
        [XmlElement("TaskPredecessor")]
        public List<TaskPredecessor> Predecessors { get; set; }

        /// <summary>
        ///     The parent task number
        /// </summary>
        public int? ParentTaskNumber { get; set; }

        /// <summary>
        ///     Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        public string ModifiedBy { get; set; }

        /// <summary>
        ///     Gets or sets the modified.
        /// </summary>
        /// <value>The modified.</value>
        public DateTime Modified { get; set; }

        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public string Group { get; set; }

        /// <summary>
        ///     Gets or sets the estimated start day number.
        /// </summary>
        /// <value>The estimated start day number.</value>
        public int? EstimatedStartDayNumber { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the estimated.
        /// </summary>
        /// <value>The duration of the estimated.</value>
        public double? EstimatedDuration { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the client barrier hours.
        /// </summary>
        /// <value>The client barrier hours.</value>
        public double? ClientBarrierHours { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        ///     Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public TaskProgressEnumDto Progress { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the creation order.
        /// </summary>
        /// <value>The creation order.</value>
        public int CreationOrder { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        public DateTime? DueDate { get; set; }

        /// <summary>
        ///     Gets or sets the percent complete.
        /// </summary>
        /// <value>The percent complete.</value>
        public double PercentComplete { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this instance is project handler restricted.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is project handler restricted; otherwise, <c>false</c>.
		/// </value>
		public bool IsProjectHandlerRestricted { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [should trigger billing].
		/// </summary>
		/// <value><c>true</c> if [should trigger billing]; otherwise, <c>false</c>.</value>
	    public bool ShouldTriggerBilling { get; set; }
    }
}
