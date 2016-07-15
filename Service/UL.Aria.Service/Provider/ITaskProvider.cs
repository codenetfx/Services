using System;
using System.Collections.Generic;

using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Interface ITaskProvider
	/// </summary>
	public interface ITaskProvider
	{


		/// <summary>
		/// Fetches the by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Project.</returns>
        Project FetchByProject(Guid projectId);

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		IList<Task> FetchAll(Guid containerId);

        /// <summary>
        ///     Fetches all keeping in flat list format, not tree with SubTasks.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>IList{Task}.</returns>
        IList<Task> FetchAllFlatList(Guid containerId);

		/// <summary>
		/// Determines if the container has pending tasks.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool PendingTasks(Guid containerId);

		/// <summary>
		///     Fetches all.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>IList{Task}.</returns>
		IList<Task> FetchAllWithDeleted(Guid containerId);


		/// <summary>
		/// Fetches the task lookup list for the specified container.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		IList<Lookup> FetchLookups(Guid containerId);

		/// <summary>
		///     Finds the by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		/// <returns>Task.</returns>
		Task FetchById(Guid containerId, Guid id);

		/// <summary>
		/// Fetches the task only.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>Task.</returns>
		Task FetchTaskOnly(Guid containerId, Guid id);

		/// <summary>
		///     Creates the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		/// <returns>Guid.</returns>
		Guid Create(Guid containerId, Task task);

		/// <summary>
		///     Updates the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="task">The task.</param>
		void Update(Guid containerId, Task task);

        /// <summary>
        /// Updates a list of tasks.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="tasks">The tasks.</param>
	    void BulkUpdate(Guid containerId, IList<Task> tasks);

		/// <summary>
		///     Deletes the specified container id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="id">The id.</param>
		void Delete(Guid containerId, Guid id);

		/// <summary>
		///     Bulks the create.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="tasks">The tasks.</param>
		void BulkCreate(Guid containerId, IList<Task> tasks);

		/// <summary>
		///     Fetches the history.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <param name="taskId">The task id.</param>
		/// <returns>IList{TaskHistory}.</returns>
		IList<TaskHistory> FetchHistory(Guid containerId, Guid taskId);

		/// <summary>
		///     Fetches the multiple container tasks.
		/// </summary>
		/// <param name="containerIds">The container ids.</param>
		/// <returns>IList{TaskContainer}.</returns>
		IList<TaskContainer> FetchMultipleContainerTasks(IEnumerable<Guid> containerIds);

		/// <summary>
		/// Fetches the delta history.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <returns></returns>
		IList<TaskDelta> FetchDeltaHistory(Guid containerId, Guid taskId);

        /// <summary>
        /// Fetches the count of tasks for the container.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
	    int FetchCount(Guid containerId);

		/// <summary>
		/// Fetches the predecessors by project.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns></returns>
		IList<TaskPredecessor> FetchPredecessorsByProject(Guid entityId);
	}
}