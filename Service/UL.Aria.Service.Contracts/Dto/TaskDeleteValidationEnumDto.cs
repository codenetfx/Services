using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Enum TaskDeleteValidationEnumDto
	/// </summary>
	[DataContract]
	public enum TaskDeleteValidationEnumDto
	{
		/// <summary>
		/// The none
		/// </summary>
		[EnumMember]
		None = 0,

		/// <summary>
		/// The task prevent deletion
		/// </summary>
		[EnumMember]
		TaskPreventDeletion = 1,

		/// <summary>
		/// The task project canceled completed
		/// </summary>
		[EnumMember]
		TaskProjectCanceledCompleted = 2,

		/// <summary>
		/// The task project user not handler
		/// </summary>
		[EnumMember]
		TaskProjectUserNotHandler = 3,

		/// <summary>
		/// The task children completed
		/// </summary>
		[EnumMember]
		TaskChildrenCompleted = 4,

		/// <summary>
		/// The task children prevent deletion
		/// </summary>
		[EnumMember]
		TaskChildrenPreventDeletion = 5
	}
}