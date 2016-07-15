using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Interface ITaskFetchStatusStrategy
	/// </summary>
	public interface ITaskFetchStatusStrategy
	{
		/// <summary>
		/// Fetches the task status.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>TaskStatusEnumDto.</returns>
		TaskStatusEnumDto FetchTaskStatus(Task task);
	}
}