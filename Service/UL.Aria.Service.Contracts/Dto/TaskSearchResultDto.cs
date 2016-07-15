using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TaskSearchResultDto : SearchResultDto
    {
        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>
        /// The project id.
        /// </value>
        [DataMember]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        [DataMember]
        public string Progress { get; set; }


        /// <summary>
        /// Gets or sets the task owner.
        /// </summary>
        /// <value>
        /// The task owner.
        /// </value>
        [DataMember]
        public string TaskOwner { get; set; }

        /// <summary>
        /// Gets or sets the task number.
        /// </summary>
        /// <value>
        /// The task number.
        /// </value>
        [DataMember]
        public int TaskNumber { get; set; }

        /// <summary>
        /// Gets or sets the actual duration.
        /// </summary>
        /// <value>
        /// The actual duration.
        /// </value>
        [DataMember]
        public double? ActualDuration { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        [DataMember]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the sub tasks.
        /// </summary>
        /// <value>
        /// The sub tasks.
        /// </value>
        [DataMember]
        public IList<TaskSearchResultDto> SubTasks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has comments.
        /// </summary>
        /// <value><c>true</c> if this instance has comments; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool HasComments { get; set; }

        /// <summary>
        /// Gets or sets the last comment.
        /// </summary>
        /// <value>
        /// The last comment.
        /// </value>
        [DataMember]
        public string LastComment { get; set; }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>
        /// The type of the task.
        /// </value>
        [DataMember]
        public Guid TaskTypeId { get; set; }
        /// <summary>
        /// Gets or sets the name of the task type.
        /// </summary>
        /// <value>
        /// The name of the task type.
        /// </value>
        [DataMember]
        public string TaskTypeName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reminder date.
        /// </summary>
        /// <value>
        /// The reminder date.
        /// </value>
        [DataMember]
        public DateTime? ReminderDate { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        [DataMember]
        public IList<SearchResultDto> Documents { get; set; }



        /// <summary>
        /// Gets or sets a value indicating whether [prevent deletion].
        /// </summary>
        /// <value><c>true</c> if [prevent deletion]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool PreventDeletion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [should trigger billing].
        /// </summary>
        /// <value><c>true</c> if [should trigger billing]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool ShouldTriggerBilling { get; set; }


        /// <summary>
        /// Gets or sets the project task should trigger billing count.
        /// </summary>
        /// <value>
        /// The project task should trigger billing count.
        /// </value>
        [DataMember]
        public int ProjectTaskShouldTriggerBillingCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is project handler restricted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is project handler restricted; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsProjectHandlerRestricted { get; set; }

        /// <summary>
        ///     Gets or sets the predecessors.
        /// </summary>
        /// <value>The predecessors.</value>
        [DataMember]
        public IList<TaskPredecessorDto> Predecessors { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        [DataMember]
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the parent task identifier.
        /// </summary>
        /// <value>
        /// The parent task identifier.
        /// </value>
        [DataMember]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the has children.
        /// </summary>
        /// <value>
        /// The has children.
        /// </value>
        [DataMember]
        public bool HasChildren { get; set; }

        /// <summary>
        /// Gets or sets the parent task number.
        /// </summary>
        /// <value>
        /// The parent task number.
        /// </value>
        [DataMember]
        public int? ParentTaskNumber { get; set; }

        /// <summary>
        /// Gets or sets the container identifier.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        [DataMember]
        public Guid? ContainerId
        {
            get
            {
                Guid result = Guid.Empty;

                if (this.Metadata.ContainsKey(AssetFieldNames.AriaContainerId))
                    Guid.TryParse(this.Metadata[AssetFieldNames.AriaContainerId], out result);

                return result;
            }
            set
            {
                this.Metadata[AssetFieldNames.AriaContainerId] = value.GetValueOrDefault().ToString();
            }
        }

        /// <summary>
        /// Gets or sets the status list.
        /// </summary>
        /// <value>
        /// The status list.
        /// </value>
        [DataMember]
        public TaskStatusListDto StatusList { get; set; }


		/// <summary>
		/// Gets or sets the task type behaviors.
		/// </summary>
		/// <value>
		/// The task type behaviors.
		/// </value>
		 [DataMember]
		public IList<TaskTypeBehaviorDto> TaskTypeBehaviors { get; set; }

         /// <summary>
         /// Gets or sets a value indicating whether this instance is free form.
         /// </summary>
         /// <value>
         /// 	<c>true</c> if this instance is free form; otherwise, <c>false</c>.
         /// </value>
        [DataMember]
         public bool IsFreeForm { get; set; }

        /// <summary>
        /// Gets or sets the child task numbers.
        /// </summary>
        /// <value>The child task numbers.</value>
        [DataMember]
        public IList<int> ChildTaskNumbers { get; set; }

    }
}