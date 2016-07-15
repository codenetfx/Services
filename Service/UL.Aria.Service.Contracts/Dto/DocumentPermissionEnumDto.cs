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
	public enum DocumentPermissionEnumDto
	{
		/// <summary>
		/// Read only to customers
		/// </summary>
		[EnumMember(Value = "ReadOnly")]
		ReadOnly,

		/// <summary>
		/// Editable to customers
		/// </summary>
		[EnumMember(Value = "Modify")]
		Modify,

		/// <summary>
		/// Hidden to customers
		/// </summary>
		[EnumMember(Value = "Private")]
		Private
	}
}
