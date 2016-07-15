using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Values for task validation results.
    /// </summary>
    [DataContract]
    public enum TaskValidationEnumDto
    {
        /// <summary>
        /// The none
        /// </summary>
        [EnumMember]
        None = 0,

        /// <summary>
        /// The children still incomplete
        /// </summary>
        [EnumMember]
        ChildrenStillIncomplete = 1,

        /// <summary>
        /// The predecessors still incomplete
        /// </summary>
        [EnumMember]
        PredecessorsStillIncomplete = 2,

        /// <summary>
        /// The task still unassigned
        /// </summary>
        [EnumMember]
        TaskStillUnassigned = 3,

        /// <summary>
        /// When awaiting assignment task must not have an owner
        /// </summary>
        [EnumMember]
        AwaitingAssignmentMustHaveNoOwner = 4,

        /// <summary>
        /// The predecessor tasks missing
        /// </summary>
        [EnumMember]
        PredecessorTasksMissing = 5,

        /// <summary>
        /// The parent task invalid
        /// </summary>
        [EnumMember]
        ParentTaskInvalid = 6,

        /// <summary>
        /// The child tasks missing
        /// </summary>
        [EnumMember]
        ChildTasksMissing = 7,

        /// <summary>
        /// The parent task child
        /// </summary>
        [EnumMember]
        ParentTaskChild = 8,

        /// <summary>
        /// The child task parent
        /// </summary>
        [EnumMember]
        ChildTaskParent = 9,

        /// <summary>
        /// The is project handler restricted
        /// </summary>
        [EnumMember]
        IsProjectHandlerRestricted = 10,

        /// <summary>
        /// Property max-length 
        /// </summary>
        [EnumMember]
        TaskNameRequired = 11,

        /// <summary>
        /// The short description maximum length
        /// </summary>
        [EnumMember]
        DescriptionMaxLength = 12,

        /// <summary>
        /// The task owner must be ul user
        /// </summary>
        [EnumMember]
        TaskOwnerMustBeUlUser = 13,

        /// <summary>
        /// Notification email addresses can only be specified on freeform tasks.
        /// </summary>
        [EnumMember]
        NotificationsRequireFreeform = 14,

        /// <summary>
        /// When project is completed or canceled, no updates are allowed to tasks.
        /// </summary>
        [EnumMember]
        TaskClosedBecauseProjectClosed = 15,

        /// <summary>
        /// Prevent non-project handlers from change task status when current status is completed or canceled.
        /// </summary>
        [EnumMember]
        ProjectHandlerCanUpdateStatusWhenClosed = 16,

		/// <summary>
		/// The task behavior restricted to project handler
		/// </summary>
		[EnumMember]
		TaskBehaviorRestrictedToProjectHandler = 17,

        /// <summary>
        /// The child task numbers restricted to projecthandler
        /// </summary>
        [EnumMember]
        ChildTaskNumbersRestrictedToProjecthandler = 18,

        /// <summary>
        /// The parent task number restricted to projecthandler
        /// </summary>
        [EnumMember]
        ParentTaskNumberRestrictedToProjecthandler = 19,

         /// <summary>
        /// The due date must be greater than start date
        /// </summary>
        [EnumMember]
        DueDateMustBeGreaterThanStartDate = 20,

        /// <summary>
        /// The task notification change
        /// </summary>
        [EnumMember]
        TaskNotificationChange = 21,

        /// <summary>
        /// The self referencing predecessor
        /// </summary>
        [EnumMember]
        SelfReferencingPredecessor = 22
    }
}