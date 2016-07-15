using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
	/// <summary>
	/// Class TaskStatusList.
	/// </summary>
	[Serializable]
	public class TaskStatusList
	{
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public TaskStatusEnumDto Status { get; set; }

		/// <summary>
		/// Gets or sets the status list.
		/// </summary>
		/// <value>The status list.</value>
		public Dictionary<TaskStatusEnumDto, string> StatusList { get; set; }
	}
}