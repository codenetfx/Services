using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyNotScheduled. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyNotScheduled : ITaskFetchStatusListStrategy
	{
		/// <summary>
		/// Fetches the task status list.
		/// </summary>
		/// <returns>Dictionary&lt;TaskStatusEnumDto, System.String&gt;.</returns>
		public Dictionary<TaskStatusEnumDto, string> FetchTaskStatusList()
		{
			return
				new Dictionary<TaskStatusEnumDto, string>
				{
					{TaskStatusEnumDto.NotScheduled, TaskStatusEnumDto.NotScheduled.ToString().SpaceIt()},
					{TaskStatusEnumDto.OnHold, TaskStatusEnumDto.OnHold.ToString().SpaceIt()},
					{TaskStatusEnumDto.Canceled, TaskStatusEnumDto.Canceled.ToString().SpaceIt()},
					{TaskStatusEnumDto.Completed, TaskStatusEnumDto.Completed.ToString().SpaceIt()}
				};
		}
	}
}