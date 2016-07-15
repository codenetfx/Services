using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Enum TaskProgressEnumDto
    /// </summary>
    [DataContract]
    public enum TaskProgressEnumDto
    {
        /// <summary>
        ///     The blocking
        /// </summary>
        [EnumMember(Value = "Waiting")] Waiting = 0,

        /// <summary>
        ///     The in trouble
        /// </summary>
        [EnumMember(Value = "InTrouble")] InTrouble = 100,

        /// <summary>
        ///     The slipping
        /// </summary>
        [EnumMember(Value = "Slipping")] Slipping = 200,

        /// <summary>
        ///     The on track
        /// </summary>
        [EnumMember(Value = "OnTrack")] OnTrack = 300,
		
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
		/// The blocking
		/// </summary>
		[EnumMember(Value = "Blocking")]
		Blocking = 700

    }
}