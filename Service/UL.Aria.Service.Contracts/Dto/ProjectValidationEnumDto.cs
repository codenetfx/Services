using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public enum ProjectValidationEnumDto
    {
        /// <summary>
        /// The none
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// The children still incomplete
        /// </summary>
        [EnumMember]
        PendingTasks = 1,

        /// <summary>
        /// The service line
        /// </summary>
        [EnumMember]
        ServiceLine =2,
        /// <summary>
        /// The industry code
        /// </summary>
        [EnumMember]
        IndustryCode =3,
    }
}