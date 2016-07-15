using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyCanceled. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyCanceled : ITaskFetchStatusListStrategy
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
					{TaskStatusEnumDto.Canceled, TaskStatusEnumDto.Canceled.ToString().SpaceIt()},
					{TaskStatusEnumDto.NotScheduled, TaskStatusEnumDto.NotScheduled.ToString().SpaceIt()},
					{TaskStatusEnumDto.InProgress, TaskStatusEnumDto.InProgress.ToString().SpaceIt()}
				};
		}
	}
}