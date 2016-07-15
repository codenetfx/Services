using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class TaskStatusListDto.
	/// </summary>
	[DataContract]
	public class TaskStatusListDto
	{
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[DataMember]
		public TaskStatusEnumDto Status { get; set; }

		/// <summary>
		/// Gets or sets the status list.
		/// </summary>
		/// <value>The status list.</value>
		[DataMember]
		public Dictionary<TaskStatusEnumDto, string> StatusList { get; set; }
	}
}