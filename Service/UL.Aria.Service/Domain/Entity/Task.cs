using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class TaskDto
	/// </summary>
    [Serializable]
	public class Task : TaskBase
	{
        private readonly Guid _restrictedToProjectHandlerBehaviorId = new Guid(TaskTypeAvailableBehaviorFieldDto.RestrictedToProjectHandlerBehavior);
        private readonly Guid _defaultTaskType = AssetFieldNames.Keys.DefaultTaskTypeId;
		/// <summary>
		///     Initializes a new instance of the <see cref="Task" /> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public Task(Guid? id)
		{
			CreateContainer = false;
			Id = id;
			Type = EntityTypeEnumDto.Task;
			Predecessors = new List<TaskPredecessor>();
			SubTasks = new List<Task>();
			Comments = new List<TaskComment>();
			StatusHistories = new List<TaskStatusHistory>();
			CompletionHistories = new List<TaskCompletionHistory>();
			ChildTaskNumbers = new List<int>();
			Progress = TaskProgressEnumDto.OnTrack;
		    Documents = new List<SearchResult>();
            PredecessorRefs = new List<Task>();
            SuccessorRefs = new List<Task>();
		    Notifications = new List<TaskNotification>();
			TaskTypeBehaviors = new List<TaskTypeBehavior>();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Task" /> class.
		/// </summary>
		public Task()
			: this(null)
		{
		}

		/// <summary>
		/// Gets or sets the primary search entity identifier.
		/// </summary>
		/// <value>The primary search entity identifier.</value>
		public Guid PrimarySearchEntityId { get; set; }

		/// <summary>
		/// Gets or sets the type of the primary search entity.
		/// </summary>
		/// <value>The type of the primary search entity.</value>
        public string PrimarySearchEntityType { get; set; }

		/// <summary>
		///     The parent id
		/// </summary>
        public Guid ParentId { get; set; }

		/// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Task Parent { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the predecessor refs.
        /// </summary>
        /// <value>
        /// The predecessor refs.
        /// </value>
        public List<Task> PredecessorRefs { get; set; }

        /// <summary>
        /// Gets or sets the successor refs.
        /// </summary>
        /// <value>
        /// The successor refs.
        /// </value>
        public List<Task> SuccessorRefs { get; set; }

        /// <summary>
        /// Gets or sets the notifications.
        /// </summary>
        /// <value>
        /// The notifications.
        /// </value>
        public List<TaskNotification> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public int Depth
        {
            get
            {
                var depth = 0;
                var temp = this.Parent;
                while (temp != null)
                {
                    temp = temp.Parent;
                    depth++;
                }

                return depth;

            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildren
        {
            get
            {
                return this.SubTasks != null && SubTasks.Count > 0;
            }
        }


		/// <summary>
		/// Gets or sets the task type behaviors.
		/// </summary>
		/// <value>
		/// The task type behaviors.
		/// </value>
		public List<TaskTypeBehavior> TaskTypeBehaviors { get; set; }
        /// <summary>
		/// Gets the status search value.
		/// </summary>
		/// <value>The status search value.</value>
        public string StatusSearchValue { get { return Status.ToSharePointString(); } }

		/// <summary>
		/// Gets or sets the progress search value.
		/// </summary>
		/// <value>The progress search value.</value>
        public string ProgressSearchValue { get { return Progress.ToSharePointString(); } }

		/// <summary>
		/// Gets or sets the child task numbers.
		/// </summary>
		/// <value>The child task numbers.</value>
		public IList<int> ChildTaskNumbers { get; set; }

		/// <summary>
		///     Gets or sets the sub tasks.
		/// </summary>
		/// <value>The sub tasks.</value>
        public IList<Task> SubTasks { get; set; }

		/// <summary>
		///     Gets or sets the comments.
		/// </summary>
		/// <value>The comments.</value>
		public IList<TaskComment> Comments { get; set; }

		/// <summary>
		/// Gets or sets the phase histories.
		/// </summary>
		/// <value>
		/// The phase histories.
		/// </value>
		public IList<TaskStatusHistory> StatusHistories { get; set; }

		/// <summary>
		/// Gets or sets the completion histories.
		/// </summary>
		/// <value>
		/// The completion histories.
		/// </value>
        public IList<TaskCompletionHistory> CompletionHistories { get; set; }

		/// <summary>
		/// Gets the status label.
		/// </summary>
		/// <value>
		/// The status label.
		/// </value>
		public string StatusLabel { get { return Status.GetDisplayName(); } }

		/// <summary>
		/// Gets the progress label.
		/// </summary>
		/// <value>
		/// The progress label.
		/// </value>
		public string ProgressLabel { get { return Progress.GetDisplayName(); } }


		/// <summary>
		/// Gets a value indicating whether this instance has task owner.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has task owner; otherwise, <c>false</c>.
		/// </value>
		public bool HasTaskOwner
		{
			get
			{
				return !string.IsNullOrWhiteSpace(TaskOwner)
					&& TaskOwner.ToLowerInvariant() != "unassigned";
			}
		}

		/// <summary>
		/// Gets the security group.
		/// </summary>
		/// <value>
		/// The security group.
		/// </value>
		protected override string SecurityGroup
		{
			get { return "Private"; }
		}

		/// <summary>
		/// Gets or sets the reminder date.
		/// </summary>
		/// <value>
		/// The reminder date.
		/// </value>
		public DateTime? ReminderDate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [is deleted].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is deleted]; otherwise, <c>false</c>.
		/// </value>
		public bool IsDeleted { get; set; }

		///// <summary>
		///// Gets or sets the completed date.
		///// </summary>
		///// <value>
		///// The completed date.
		///// </value>
		//public DateTime? CompletedDate { get; set; }

		/// <summary>
		///     Serializes the parent.
		/// </summary>
		/// <param name="xmlWriter">The XML writer.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <param name="isContainerOnly"></param>
		protected override void SerializeParent(XmlWriter xmlWriter, object parent, IAssetFieldMetadata assetFieldMetadata, bool isContainerOnly = false)
		{
			if (string.IsNullOrEmpty(TaskOwner))
				TaskOwner = "Unassigned";

			base.SerializeParent(xmlWriter, parent, assetFieldMetadata, isContainerOnly);
		}

		/// <summary>
		/// Gets the completion date.
		/// </summary>
		/// <value>
		/// The completion date.
		/// </value>
		public DateTime? CompletedDate
		{
			get
			{
				if (CompletionHistories == null ||
					Status != TaskStatusEnumDto.Completed)
				{
					return null;
				}

				TaskStatusHistory completed = null;
				foreach (var taskStatusHistory in StatusHistories.OrderBy(x => x.CreatedDate))
				{
					if (taskStatusHistory.Status != Status)
					{
						completed = null; //reset
					}
					else if (null == completed)
					{
						completed = taskStatusHistory;
					}
				}

				if (null != completed)
					return completed.CreatedDate;
				return UpdatedDateTime == DateTime.MinValue ? DateTime.UtcNow : UpdatedDateTime;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has comments.
		/// </summary>
		/// <value><c>true</c> if this instance has comments; otherwise, <c>false</c>.</value>
		public bool HasComments { get; set; }

        /// <summary>
        /// Gets or sets the last comment.
        /// </summary>
        /// <value>
        /// The last comment.
        /// </value>
        public string LastComment { get; set; }

		/// <summary>
		/// Gets or sets the Last Document Added.
		/// </summary>
		/// <value>The LastDocumentAdded.</value>
		public string LastDocumentAdded { get; set; }

		/// <summary>
		/// Gets or sets the Last Document Removed.
		/// </summary>
		/// <value>The LastDocumentRemoved.</value>
        public string LastDocumentRemoved { get; set; }

		/// <summary>
		/// Gets or sets the type of the task.
		/// </summary>
		/// <value>
		/// The type of the task.
		/// </value>
		public Guid? TaskTypeId { get; set; }

		/// <summary>
		/// Gets or sets the name of the task type.
		/// </summary>
		/// <value>
		/// The name of the task type.
		/// </value>
		public string TaskTypeName { get; set; }



		/// <summary>
		/// Gets or sets the order number.
		/// </summary>
		/// <value>
		/// The order number.
		/// </value>
		public string OrderNumber { get; set; }
		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		/// <value>
		/// The company.
		/// </value>
		public string CompanyName { get; set; }


		/// <summary>
		/// Gets or sets the task template identifier.
		/// </summary>
		/// <value>
		/// The task template identifier.
		/// </value>
		public Guid? TaskTemplateId { get; set; }


        /// <summary>
        /// Gets or sets the Record Version.
        /// </summary>
        /// <value>
        /// Timestamp.
        /// </value>
        public byte[] RecordVersion { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        public IList<SearchResult> Documents { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [prevent deletion].
		/// </summary>
		/// <value><c>true</c> if [prevent deletion]; otherwise, <c>false</c>.</value>
		public bool PreventDeletion { get; set; }

		/// <summary>
		/// Gets or sets the project task should trigger billing count.
		/// </summary>
		/// <value>The project task should trigger billing count.</value>
		public int ProjectTaskShouldTriggerBillingCount { get; set; }

        /// <summary>
        /// Gets or sets the status list.
        /// </summary>
        /// <value>
        /// The status list.
        /// </value>
	    public TaskStatusList StatusList { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is re activate request.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is re activate request; otherwise, <c>false</c>.
		/// </value>
		public bool IsReActivateRequest  { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is closed; otherwise, <c>false</c>.
        /// </value>
	    public bool IsClosed
	    {
	        get { return Status == TaskStatusEnumDto.Completed || Status == TaskStatusEnumDto.Canceled; }
	    }

        /// <summary>
        /// Gets a value indicating whether this instance is free form.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is free form; otherwise, <c>false</c>.
        /// </value>
	    public bool IsFreeForm
	    {
	        get
	        {
	            return this.TaskTypeId.GetValueOrDefault() == Guid.Empty 
                    || this.TaskTypeId.GetValueOrDefault() == _defaultTaskType;
	        }
	    }

        /// <summary>
        /// Gets a value indicating whether this instance is parent that has child project handler parent task number field restriced.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is parent that has child project handler parent task number field restriced; otherwise, <c>false</c>.
        /// </value>
        public bool IsChildTaskNumbersRestrictedByRelationship
	    {
	        get
	        {
                if (null == SubTasks || SubTasks.Count() <= 0)
	            {
	                return false;
	            }

                return SubTasks.Any(x => x.TaskTypeBehaviors
                    .Any(y => y.FieldName == "ParentTaskNumber"
                     && y.TaskTypeAvailableBehaviorId == _restrictedToProjectHandlerBehaviorId));
	        }
	    }

        /// <summary>
        /// Gets a value indicating whether this instance is child that has parent project hanlder child task number field restriced.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is child that has parent project hanlder child task number field restriced; otherwise, <c>false</c>.
        /// </value>
        public bool IsParentTaskNumberRestrictedByRelationship	   
        {
	        get
	        {
                if (null == Parent || null == Parent.TaskTypeBehaviors || Parent.TaskTypeBehaviors.Count() <=0)
	            {
	                return false;
	            }
	            return Parent.TaskTypeBehaviors.Any(x => x.FieldName == "ChildTaskNumbers"
                        && x.TaskTypeAvailableBehaviorId == _restrictedToProjectHandlerBehaviorId);
	        }
	    }
	}
}