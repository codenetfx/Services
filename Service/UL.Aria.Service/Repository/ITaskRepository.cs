using System;
using System.Collections.Generic;

using UL.Aria.Service.Auditing;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Interface ITaskRepository
	/// </summary>
	[Audit]
	public interface ITaskRepository : IRepositoryBase<Task>
	{
		/// <summary>
		/// Creates the specified task.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>Guid.</returns>
		[AuditResource("task", ActionType = "Created")]
		Guid Create(Task task);


        /// <summary>
        /// Fetches the by project.
        /// </summary>
        /// <param name="ProjectId">The project identifier.</param>
        /// <returns></returns>
        [AuditIgnore]
        List<Task> FetchByProject(Guid ProjectId);

        /// <summary>
        /// Fetches the predecessors by project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        [AuditIgnore]
        List<TaskPredecessor> FetchPredecessorsByProject(Guid projectId);

		/// <summary>
		/// Updates the specified task.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>System.Int32.</returns>
		[AuditResource("task", ActionType = "Updated")]
		new int Update(Task task);

		/// <summary>
		/// Finds the by identifier task only.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Task.</returns>
		[AuditIgnore]
		Task FindByIdTaskOnly(Guid entityId);

		/// <summary>
		/// Finds the ids by primary search entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
		/// <returns>List&lt;Lookup&gt;.</returns>
		[AuditIgnore]
		List<Lookup> FindIdsByPrimarySearchEntityId(Guid primarySearchEntityId, bool includeDeleted = false);

		/// <summary>
		/// Finds the by primary search entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="flatList">if set to <c>true</c> [return flat list of tasks]</param>
		/// <returns>List&lt;Task&gt;.</returns>
		[AuditIgnore]
        List<Task> FindByPrimarySearchEntityId(Guid primarySearchEntityId, bool includeDeleted = false, bool flatList = false);

		/// <summary>
		/// Finds the by multiple primary search entity ids.
		/// </summary>
		/// <param name="primarySearchEntityIds">The primary search entity ids.</param>
		/// <returns>List&lt;Task&gt;.</returns>
		[AuditIgnore]
		List<Task> FindByMultiplePrimarySearchEntityIds(IList<Guid> primarySearchEntityIds);

		/// <summary>
		/// Removes the specified entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <param name="userId">The user identifier.</param>
		[AuditResource("entityId", ActionType = "Deleted")]
		void Remove(Guid entityId, Guid userId);

		/// <summary>
		/// Bulk create tasks.
		/// </summary>
		/// <param name="tasks">The tasks.</param>
		[AuditResource("tasks", ActionType = "Created")]
		void BulkCreate(IEnumerable<Task> tasks);

        /// <summary>
        /// Bulk update tasks.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        [AuditResource("tasks", ActionType = "Updated")]
		void BulkUpdate(IEnumerable<Task> tasks);

		/// <summary>
		/// Determines if there are pending tasks for the given entity.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[AuditIgnore]
		bool PendingTasks(Guid primarySearchEntityId);

	    /// <summary>
	    /// Gets the count of tasks for the pr.
	    /// </summary>
	    /// <param name="primarySearchEntityId">The entity identifier.</param>
	    /// <returns></returns>
	    /// <exception cref="UL.Enterprise.Foundation.Data.DatabaseItemNotFoundException"></exception>
        [AuditIgnore]
        int FetchCountByPrimarySearchEntityId(Guid primarySearchEntityId);
	}
}