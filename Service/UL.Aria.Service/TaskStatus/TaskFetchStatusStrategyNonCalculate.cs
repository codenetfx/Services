using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusStrategyMirror.
	/// </summary>
	public sealed class TaskFetchStatusStrategyNonCalculate : ITaskFetchStatusStrategy
	{
		/// <summary>
		/// Fetches the task status.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>UL.Aria.Service.Contracts.Dto.TaskStatusEnumDto.</returns>
		public TaskStatusEnumDto FetchTaskStatus(Task task)
		{
			return task.Status;
		}
	}
}