using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.DataRule
{
    /// <summary>
    /// Data context
    /// </summary>
    public interface IDataRuleContext { }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TParent">The type of the parent.</typeparam>
    /// <typeparam name="T"></typeparam>
    public interface IDataRuleContext<TParent, T> : IDataRuleContext where TParent : ITrackedDomainEntity where T : ITrackedDomainEntity
    {
        /// <summary>
        /// Gets the synchronize errors.
        /// </summary>
        /// <value>
        /// The synchronize errors.
        /// </value>
        IDictionary<Guid, IList<TaskValidationEnumDto>> SyncErrors { get; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        TParent OriginalProject { get;  }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        TParent ActiveProject { get; }

        /// <summary>
        /// Gets or sets the process stack.
        /// </summary>
        /// <value>
        /// The process stack.
        /// </value>
        Queue<T> ProcessingQueue { get; }

        /// <summary>
        /// Gets the original tasks2.
        /// </summary>
        /// <value>
        /// The original tasks2.
        /// </value>
        Dictionary<Guid?, T> OriginalTasks { get; }

        /// <summary>
        /// Gets the user altered task ids.
        /// </summary>
        /// <value>
        /// The user altered task ids.
        /// </value>
        IEnumerable<Guid> UserAlteredTaskIds { get; }

        /// <summary>
        /// Gets the working tasks.
        /// </summary>
        /// <value>
        /// The working tasks.
        /// </value>
        IDictionary<Guid?, T> WorkingTasks { get; }


        /// <summary>
        /// Gets or sets the state of the workflow.
        /// </summary>
        /// <value>
        /// The state of the workflow.
        /// </value>
        IDictionary<string, object> WorkflowState { get; set; }

        /// <summary>
        /// Records the task update.
        /// </summary>
        /// <param name="updatedTask">The updated task.</param>
        void RecordEntityUpdate(Domain.Entity.Task updatedTask);


        /// <summary>
        /// Gets the updated entities.
        /// </summary>
        /// <param name="flatten">if set to <c>true</c> [flatten].</param>
        /// <returns></returns>
        List<T> GetUpdatedEntities(bool flatten=true);


        /// <summary>
        /// Determines whether [has dirty entities].
        /// </summary>
        /// <returns></returns>
        bool HasDirtyEntities();

        /// <summary>
        /// Records the task update.
        /// </summary>
        /// <param name="deletedTask">The deleted task.</param>
        void RecordEntityDelete(Domain.Entity.Task deletedTask);

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        /// <returns></returns>
        List<T> GetDeletedEntities();

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active project dirty.
		/// </summary>
		/// <value><c>true</c> if this instance is active project dirty; otherwise, <c>false</c>.</value>
		bool IsActiveProjectDirty { get; set; }

		/// <summary>
		/// Gets the updated parent.
		/// </summary>
		/// <returns>TParent.</returns>
	    TParent GetUpdatedParent();
    }
}