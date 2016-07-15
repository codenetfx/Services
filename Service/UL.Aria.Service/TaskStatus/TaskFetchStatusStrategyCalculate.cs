using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusStrategyCalculate. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusStrategyCalculate : ITaskFetchStatusStrategy
	{
		/// <summary>
		/// Fetches the task status.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>TaskStatusEnumDto.</returns>
		public TaskStatusEnumDto FetchTaskStatus(Task task)
		{
			if (task.StartDate == null)
			{
				return TaskStatusEnumDto.NotScheduled;
			}
			if (!task.HasTaskOwner)
			{
				return TaskStatusEnumDto.AwaitingAssignment;
			}
			// (task.StartDate != null && task.HasTaskOwner)
			return TaskStatusEnumDto.InProgress;
		}
	}
}