using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Data transfer object for user business claims.
    /// </summary>
    [DataContract]
    public class UserBusinessClaimDto
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        [DataMember]
        public string LoginId { get; set; }


        /// <summary>
        /// Gets or sets the claim id.
        /// </summary>
        /// <value>
        /// The claim id.
        /// </value>
        [DataMember]
        public BusinessClaimDto Claim { get; set; }
    }
}
