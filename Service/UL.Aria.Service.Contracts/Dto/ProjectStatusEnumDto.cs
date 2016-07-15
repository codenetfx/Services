using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class ProjectStatusEnumDto
    /// </summary>
    [DataContract]
    public enum ProjectStatusEnumDto
    {
        /// <summary>
        ///     The in progress
        /// </summary>
        [Display(Name = "In Progress")]
        [EnumMember(Value = "InProgress")]
        InProgress,

        /// <summary>
        ///     The on hold
        /// </summary>
        [Display(Name = "On Hold")]
        [EnumMember(Value = "OnHold")]
        OnHold,

        /// <summary>
        ///     The completed
        /// </summary>
        [EnumMember(Value = "Completed")] Completed,

        /// <summary>
        ///     The canceled
        /// </summary>
        [EnumMember(Value = "Canceled")] Canceled
    }
}