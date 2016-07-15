using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// User Profile: Edit Profile, Basic Information
    /// </summary>
    public class ProfileEditBasicBo
    {
        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        /// <value>
        /// The user display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the jobtitle.
        /// </summary>
        /// <value>
        /// The jobtitle.
        /// </value>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the about me.
        /// </summary>
        /// <value>
        /// The about me.
        /// </value>
        public string AboutMe { get; set; }

        /// <summary>
        /// Gets or sets the login id.
        /// </summary>
        /// <value>
        /// The login id.
        /// </value>
        public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the profile id.
        /// </summary>
        /// <value>
        /// The profile id.
        /// </value>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the modifying user.
        /// </summary>
        /// <value>
        /// The modifying user.
        /// </value>
        public Guid ModifyingUser { get; set; }

        /// <summary>
        /// Gets or sets the company external id.
        /// </summary>
        /// <value>
        /// The company external id.
        /// </value>
        public string CompanyExternalId { get; set; }

    }
}
