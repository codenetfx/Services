using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
	///     Enum TaskStatusEnumDto
	/// </summary>
	[DataContract]
	public enum TaskStatusEnumDto
	{
		/// <summary>
		///     The not scheduled
		/// </summary>
		[EnumMember(Value = "NotScheduled")]
		NotScheduled = 0,

		/// <summary>
		///     The not started
		/// </summary>
		[EnumMember(Value = "NotStarted")]
		NotStarted = 100,

		/// <summary>
		/// The awaiting assignment
		/// </summary>
		[EnumMember(Value = "AwaitingAssignment")]
		AwaitingAssignment = 150,

		/// <summary>
		///     The in progress
		/// </summary>
		[EnumMember(Value = "InProgress")]
		InProgress = 200,

		/// <summary>
		///     The on hold
		/// </summary>
		[EnumMember(Value = "OnHold")]
		OnHold = 300,

		/// <summary>
		///     The completed
		/// </summary>
		[EnumMember(Value = "Completed")]
		Completed = 400,

		/// <summary>
		///     The canceled
		/// </summary>
		[EnumMember(Value = "Canceled")]
		Canceled = 500,

		/// <summary>
		/// The remove hold
		/// </summary>
		[EnumMember(Value = "RemoveHold")]
		RemoveHold = 600

        
	}
}