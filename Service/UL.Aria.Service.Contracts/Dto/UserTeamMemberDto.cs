using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Class UserTeamDto
    /// </summary>
    [DataContract]
    public class UserTeamMemberDto : TrackedEntityDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user login identifier.
        /// </summary>
        /// <value>
        /// The user login identifier.
        /// </value>
        [DataMember]
        public string UserLoginId { get; set; }

        /// <summary>
        /// Gets or sets the user team identifier.
        /// </summary>
        /// <value>
        /// The user team identifier.
        /// </value>
        [DataMember]
        public Guid UserTeamId { get; set; }

        /// <summary>
        /// Gets or sets the included user team identifier.
        /// </summary>
        /// <value>
        /// The included user team identifier.
        /// </value>
        [DataMember]
        public Guid? IncludedUserTeamId { get; set; }
    }
}
