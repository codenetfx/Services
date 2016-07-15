using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Interface ITaskFetchStatusListStrategy
	/// </summary>
	public interface ITaskFetchStatusListStrategy
	{
		/// <summary>
		/// Fetches the task status list.
		/// </summary>
		/// <returns>Dictionary&lt;TaskStatusEnumDto, System.String&gt;.</returns>
		Dictionary<TaskStatusEnumDto, string> FetchTaskStatusList();
	}
}