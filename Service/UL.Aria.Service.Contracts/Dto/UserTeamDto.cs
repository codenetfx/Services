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
    public class UserTeamDto : TrackedEntityDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeamDto"/> class.
        /// </summary>
        public UserTeamDto()
        {
            TeamMembers = new List<UserTeamMemberDto>();
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team members.
        /// </summary>
        /// <value>
        /// The team members.
        /// </value>
        [DataMember]
        public IList<UserTeamMemberDto> TeamMembers { get; set; }
    }
}
