using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskFetchStatusReActivateStrategy : ITaskFetchStatusStrategy
	{
		/// <summary>
		/// Fetches the task status.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns>UL.Aria.Service.Contracts.Dto.TaskStatusEnumDto.</returns>
		public TaskStatusEnumDto FetchTaskStatus(Task task)
		{
			if (task.IsReActivateRequest)
			{
				if (task.StartDate == null)
				{
					return TaskStatusEnumDto.NotScheduled;
				}
				return TaskStatusEnumDto.InProgress;
			}
			return task.Status;
		}
	}
}
