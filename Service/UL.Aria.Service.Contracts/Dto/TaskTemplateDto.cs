using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class TaskTemplateDto
    /// </summary>
    [DataContract]
    public class TaskTemplateDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTemplateDto"/> class.
        /// </summary>
        public TaskTemplateDto()
        {
            SubTasks = new List<TaskTemplateDto>();
            Predecessors = new List<TaskPredecessorDto>();
            Status = TaskStatusEnumDto.NotScheduled.ToString();
            Progress = TaskProgressEnumDto.OnTrack.ToString();
        }


        /// <summary>
        /// Gets or sets the project template identifier.
        /// </summary>
        /// <value>
        /// The project template identifier.
        /// </value>
        [DataMember]
        public Guid ProjectTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        ///     Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        [DataMember]
        public int TaskNumber { get; set; }

        /// <summary>
        ///     The parent task number
        /// </summary>
        [DataMember]
        public int? ParentTaskNumber { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the due date.
        /// </summary>
        /// <value>The due date.</value>
        [DataMember]
        public DateTime? DueDate { get; set; }

        /// <summary>
        ///     Gets or sets the percent complete.
        /// </summary>
        /// <value>The percent complete.</value>
        [DataMember]
        public double PercentComplete { get; set; }

        /// <summary>
        ///     Gets or sets the actual duration.
        /// </summary>
        /// <value>The actual duration.</value>
        [DataMember]
        public double? ActualDuration { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        ///     Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        [DataMember]
        public string Progress { get; set; }

        /// <summary>
        ///     Gets or sets the task owner.
        /// </summary>
        /// <value>The task owner.</value>
        [DataMember]
        public string TaskOwner { get; set; }

        /// <summary>
        ///     Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        ///     Gets or sets the modified.
        /// </summary>
        /// <value>The modified.</value>
        [DataMember]
        public DateTime Modified { get; set; }

        /// <summary>
        ///     Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        [DataMember]
        public string ModifiedBy { get; set; }

        /// <summary>
        ///     Gets or sets the client barrier hours.
        /// </summary>
        /// <value>The client barrier hours.</value>
        [DataMember]
        public double? ClientBarrierHours { get; set; }

        /// <summary>
        ///     Gets or sets the predecessors.
        /// </summary>
        /// <value>The predecessors.</value>
        [DataMember]
        public List<TaskPredecessorDto> Predecessors { get; set; }

        /// <summary>
        ///     Gets or sets the sub tasks.
        /// </summary>
        /// <value>The sub tasks.</value>
        [DataMember]
        public List<TaskTemplateDto> SubTasks { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the estimated.
        /// </summary>
        /// <value>The duration of the estimated.</value>
        [DataMember]
        public double? EstimatedDuration { get; set; }

        /// <summary>
        ///     Gets or sets the estimated start day number.
        /// </summary>
        /// <value>The estimated start day number.</value>
        [DataMember]
        public int? EstimatedStartDayNumber { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public EntityTypeEnumDto Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the task type identifier.
        /// </summary>
        /// <value>
        /// The task type identifier.
        /// </value>
        [DataMember]
        public Guid? TaskTypeId { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this instance is project handler restricted.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is project handler restricted; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsProjectHandlerRestricted { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [should trigger billing].
		/// </summary>
		/// <value><c>true</c> if [should trigger billing]; otherwise, <c>false</c>.</value>
		[DataMember]
		public bool ShouldTriggerBilling { get; set; }
	}
}
