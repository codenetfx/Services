using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// 
	/// </summary>
	[DataContract]
	public class TaskDeleteValidationDto
	{

		/// <summary>
		/// Gets or sets the name of the task.
		/// </summary>
		/// <value>
		/// The name of the task.
		/// </value>
		[DataMember]
		public string TaskName { get; set; }
		/// <summary>
		/// Gets or sets the task delete validation enum.
		/// </summary>
		/// <value>
		/// The task delete validation enum.
		/// </value>
		[DataMember]
		public TaskDeleteValidationEnumDto TaskDeleteValidationEnum { get; set; }
	}
}
