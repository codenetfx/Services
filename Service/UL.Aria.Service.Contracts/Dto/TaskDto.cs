using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class TaskDto
    /// </summary>
    [DataContract]
    public class TaskDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDto"/> class.
        /// </summary>
        public TaskDto()
        {
            Comments = new List<TaskCommentDto>();
            StatusHistories = new List<TaskStatusHistoryDto>();
            CompletionHistories = new List<TaskCompletionHistoryDto>();
            Predecessors = new List<TaskPredecessorDto>();
			SubTasks = new List<TaskDto>();
			ChildTaskNumbers = new List<int>();
            Notifications = new List<TaskNotificationDto>();
        }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember]
        public Guid? Id { get; set; }

		/// <summary>
		/// Gets or sets the primary search entity identifier.
		/// </summary>
		/// <value>The primary search entity identifier.</value>
		[DataMember]
		public Guid PrimarySearchEntityId { get; set; }

		/// <summary>
		/// Gets or sets the type of the primary search entity.
		/// </summary>
		/// <value>The type of the primary search entity.</value>
		[DataMember]
		public string PrimarySearchEntityType { get; set; }

        /// <summary>
        ///     Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        [DataMember]
        public int TaskNumber { get; set; }

        /// <summary>
        ///     The parent id
        /// </summary>
        [DataMember]
        public Guid ParentId { get; set; }

		/// <summary>
		/// Gets or sets the parent task number.
		/// </summary>
		/// <value>The parent task number.</value>
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
        public TaskStatusEnumDto Status { get; set; }

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
        public TaskProgressEnumDto Progress { get; set; }

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
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [DataMember]
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        [DataMember]
        public string ModifiedBy { get; set; }

        /// <summary>
        ///     Gets or sets the user it was created by.
        /// </summary>
        /// <value>
        ///     The created by.
        /// </value>
        [DataMember]
        public Guid CreatedById { get; set; }

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
        public IList<TaskPredecessorDto> Predecessors { get; set; }

		/// <summary>
		/// Gets or sets the child task numbers.
		/// </summary>
		/// <value>The child task numbers.</value>
		[DataMember]
		public IList<int> ChildTaskNumbers { get; set; }

        /// <summary>
        ///     Gets or sets the sub tasks.
        /// </summary>
        /// <value>The sub tasks.</value>
        [DataMember]
        public IList<TaskDto> SubTasks { get; set; }

        /// <summary>
        ///     Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        ///     Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [DataMember]
        public IList<TaskCommentDto> Comments { get; set; }

        /// <summary>
        /// Gets or sets the phase histories.
        /// </summary>
        /// <value>
        /// The phase histories.
        /// </value>
        [DataMember]
        public IList<TaskStatusHistoryDto> StatusHistories { get; set; }

        /// <summary>
        /// Gets or sets the completion histories.
        /// </summary>
        /// <value>
        /// The completion histories.
        /// </value>
        [DataMember]
        public IList<TaskCompletionHistoryDto> CompletionHistories { get; set; }

        /// <summary>
        /// Gets or sets the notifications.
        /// </summary>
        /// <value>
        /// The notifications.
        /// </value>
        [DataMember]
        public IList<TaskNotificationDto> Notifications { get; set; }

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
        /// Gets or sets the reminder date.
        /// </summary>
        /// <value>
        /// The reminder date.
        /// </value>
        [DataMember]
        public DateTime? ReminderDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is deleted].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is deleted]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the completed date.
        /// </summary>
        /// <value>
        /// The completed date.
        /// </value>
        [DataMember]
        public DateTime? CompletedDate { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance has comments.
		/// </summary>
		/// <value><c>true</c> if this instance has comments; otherwise, <c>false</c>.</value>
		[DataMember]
		public bool HasComments { get; set; }

        /// <summary>
        /// Gets or sets the Last Document Added.
        /// </summary>
        /// <value>The LastDocumentAdded.</value>
        [DataMember]
        public string LastDocumentAdded { get; set; }

        /// <summary>
        /// Gets or sets the Last Document Removed.
        /// </summary>
        /// <value>The LastDocumentRemoved.</value>
        [DataMember]
        public string LastDocumentRemoved { get; set; }

		/// <summary>
		/// Gets or sets the type of the task.
		/// </summary>
		/// <value>
		/// The type of the task.
		/// </value>
		[DataMember]
		public Guid? TaskTypeId { get; set; }
		/// <summary>
		/// Gets or sets the name of the task type.
		/// </summary>
		/// <value>
		/// The name of the task type.
		/// </value>
		[DataMember]
		public string TaskTypeName { get; set; }




		/// <summary>
		/// Gets or sets a value indicating whether this instance is project handler restricted.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is project handler restricted; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsProjectHandlerRestricted { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the task template identifier.
		/// </summary>
		/// <value>
		/// The task template identifier.
		/// </value>
		[DataMember]
		public Guid? TaskTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the Record Version.
        /// </summary>
        /// <value>
        /// Timestamp.
        /// </value>
        [DataMember]
        public byte[] RecordVersion { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [should trigger billing].
		/// </summary>
		/// <value><c>true</c> if [should trigger billing]; otherwise, <c>false</c>.</value>
		[DataMember]
		public bool ShouldTriggerBilling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [prevent deletion].
		/// </summary>
		/// <value><c>true</c> if [prevent deletion]; otherwise, <c>false</c>.</value>
		[DataMember]
		public bool PreventDeletion { get; set; }

		/// <summary>
		/// Gets or sets the project task should trigger billing count.
		/// </summary>
		/// <value>The project task should trigger billing count.</value>
		[DataMember]
		public int ProjectTaskShouldTriggerBillingCount { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is re activate request.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is re activate request; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsReActivateRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is free form.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is free form; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsFreeForm { get; set; }
	}
}