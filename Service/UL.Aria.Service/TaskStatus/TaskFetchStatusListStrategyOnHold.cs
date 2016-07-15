using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyOnHold. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyOnHold : ITaskFetchStatusListStrategy
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
					{TaskStatusEnumDto.OnHold, TaskStatusEnumDto.OnHold.ToString().SpaceIt()},
					{TaskStatusEnumDto.RemoveHold, TaskStatusEnumDto.RemoveHold.ToString().SpaceIt()},
					{TaskStatusEnumDto.Canceled, TaskStatusEnumDto.Canceled.ToString().SpaceIt()},
					{TaskStatusEnumDto.Completed, TaskStatusEnumDto.Completed.ToString().SpaceIt()}
				};
		}
	}
}