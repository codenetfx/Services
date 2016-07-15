using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{

    /// <summary>
    /// Provides a Type enumeration for behavioral relations to Notification types.
    /// </summary>
    [DataContract]
    public enum NotificationTypeDto
    {
        /// <summary>
        /// The undefined
        /// </summary>
        [Display(Name = "")]
        [EnumMember(Value = "Undefined")]
        Undefined = 0,

        /// <summary>
        /// Identifies a notification as based on a task's reminder.
        /// </summary>
        [Display(Name = "Task Coming Due")]
        [EnumMember(Value = "TaskReminder")]
        TaskReminder = 1,

        /// <summary>
        /// Identifies a notification as based on a task's due date.
        /// </summary>
        [Display(Name = "Task Overdue")]
        [EnumMember(Value = "TaskDueDate")]
        TaskDueDate = 2,

        /// <summary>
        /// Identifies a notification as based on a task's comments.
        /// </summary>
        [Display(Name = "Task Comment")]
        [EnumMember(Value = "TaskComment")]
        TaskComment = 3,

        /// <summary>
        /// Identifies the entity cleanup strategy, 
        /// this is not a real notification type.
        /// </summary>
        [Display(Name = "Cleanup")]
        [EnumMember(Value = "EntityCleanup")]
        EntityCleanup = 4,

        /// <summary>
        /// Identifies a notification as based on task's reminder removal.
        /// </summary>
        [Display(Name = "Task Not Coming Due")]
        [EnumMember(Value = "TaskReminderRemoval")]
        TaskReminderRemoval = 5,

		/// <summary>
		/// The order on hold
		/// </summary>
		[Display(Name = "Order On Hold")]
		[EnumMember(Value = "OrderOnHold")]
		OrderOnHold = 6,

		/// <summary>
		/// The order canceled
		/// </summary>
		[Display(Name = "Order Canceled")]
		[EnumMember(Value = "OrderCanceled")]
		OrderCanceled = 7,

		/// <summary>
		/// The order line on hold
		/// </summary>
		[Display(Name = "Order Line On Hold")]
		[EnumMember(Value = "OrderLineOnHold")]
		OrderLineOnHold = 8,

		/// <summary>
		/// The order line canceled
		/// </summary>
		[Display(Name = "Order Line Canceled")]
		[EnumMember(Value = "OrderLineCanceled")]
		OrderLineCanceled = 9,

		/// <summary>
		/// The order cleanup
		/// </summary>
		[Display(Name = "Order Cleanup")]
		[EnumMember(Value = "OrderCleanup")]
		OrderCleanup = 10,

		/// <summary>
		/// The order line cleanup
		/// </summary>
		[Display(Name = "Order Line Cleanup")]
		[EnumMember(Value = "OrderLineCleanup")]
		OrderLineCleanup = 11,

		/// <summary>
		/// The project handler
		/// </summary>
		[Display(Name = "Project Handler Change")]
		[EnumMember(Value = "ProjectHandlerChange")]
		ProjectHandlerChange = 12,

		/// <summary>
		/// The task successor status set
		/// </summary>
		[Display(Name = "Task Successor Status Set")]
		[EnumMember(Value = "TaskSuccessorStatusSet")]
		TaskSuccessorStatusSet = 13,
	}
}
