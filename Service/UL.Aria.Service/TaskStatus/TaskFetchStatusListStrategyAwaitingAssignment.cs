using System.Collections.Generic;
using System.Text.RegularExpressions;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyAwaitingAssignment. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyAwaitingAssignment : ITaskFetchStatusListStrategy
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
					{TaskStatusEnumDto.AwaitingAssignment, TaskStatusEnumDto.AwaitingAssignment.ToString().SpaceIt()},
					{TaskStatusEnumDto.OnHold, TaskStatusEnumDto.OnHold.ToString().SpaceIt()},
					{TaskStatusEnumDto.Canceled, TaskStatusEnumDto.Canceled.ToString().SpaceIt()},
					{TaskStatusEnumDto.Completed, TaskStatusEnumDto.Completed.ToString().SpaceIt()}
				};
		}
	}
}