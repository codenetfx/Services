using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ProfileBasicDto
    {
        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        /// <value>
        /// The user display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the jobtitle.
        /// </summary>
        /// <value>
        /// The jobtitle.
        /// </value>
        [DataMember]
        public string Jobtitle { get; set; }
        /// <summary>
        /// Gets or sets the about me.
        /// </summary>
        /// <value>
        /// The about me.
        /// </value>
        [DataMember]
        public string AboutMe { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        [DataMember]
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        /// <value>
        /// The profile id.
        /// </value>
        [DataMember]
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the company id.
        /// </summary>
        /// <value>
        /// The company id.
        /// </value>
        [DataMember]
        public string CompanyExternalId { get; set; }

        /// <summary>
        /// Gets or sets the modifying user.
        /// </summary>
        /// <value>
        /// The modifying user.
        /// </value>
        [DataMember]
        public Guid ModifyingUser { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// <value>
        /// The employee identifier.
        /// </value>
        [DataMember]
        public string EmployeeId { get; set; }
    }
}