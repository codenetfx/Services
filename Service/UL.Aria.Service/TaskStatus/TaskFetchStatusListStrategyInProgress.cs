using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyInProgress. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyInProgress : ITaskFetchStatusListStrategy
	{
		/// <summary>
		/// Fetches the task status list.
		/// </summary>
		/// <returns>Dictionary&lt;TaskStatusEnumDto, System.String&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Dictionary<TaskStatusEnumDto, string> FetchTaskStatusList()
		{
			return
				new Dictionary<TaskStatusEnumDto, string>
				{
					{TaskStatusEnumDto.InProgress, TaskStatusEnumDto.InProgress.ToString().SpaceIt()},
					{TaskStatusEnumDto.OnHold, TaskStatusEnumDto.OnHold.ToString().SpaceIt()},
					{TaskStatusEnumDto.Canceled, TaskStatusEnumDto.Canceled.ToString().SpaceIt()},
					{TaskStatusEnumDto.Completed, TaskStatusEnumDto.Completed.ToString().SpaceIt()}
				};
		}
	}
}