using System;
using System.Collections.Generic;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Profile business class.
    /// </summary>
    [Serializable]
    public sealed class ProfileBo : TrackedDomainEntity
    {
        /// <summary>
        ///     Gets or sets the user's company display name.
        /// </summary>
        /// <value>
        ///     The company display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the login id.
        /// </summary>
        /// <value>
        ///     The login id.
        /// </value>
        public string LoginId { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>
        ///     The company id.
        /// </value>
        public Guid CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the claims.
        /// </summary>
        /// <value>
        ///     The claims.
        /// </value>
        public IList<BusinessClaim> Claims { get; set; }

        /// <summary>
        ///     Gets or sets the company external id.
        /// </summary>
        /// <value>
        ///     The company external id.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public string CompanyExternalId { get; set; }

        /// <summary>
        ///     Gets or sets the about me details for the user.
        /// </summary>
        /// <value>
        ///     The user's about me details.
        /// </value>
        public string AboutMe { get; set; }

		/// <summary>
		/// Gets or sets the name of the company.
		/// </summary>
		/// <value>
		/// The name of the company.
		/// </value>
		public string CompanyName { get; set; }

    }
}