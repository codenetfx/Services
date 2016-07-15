using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{

    /// <summary>
    /// Defines severity of validation violations.
    /// </summary>
    [DataContract]
    public enum ValidationLevelEnumDto
    {
        /// <summary>
        /// The warning
        /// </summary>
        [EnumMember(Value ="Warning")]
        Warning = 0,

        /// <summary>
        /// The error
        /// </summary>
        [EnumMember(Value="Error")]
        Error = 1,
    }
}
