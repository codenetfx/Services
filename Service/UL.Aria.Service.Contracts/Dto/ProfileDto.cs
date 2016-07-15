using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Profile data transfer object class.
    /// </summary>
    [DataContract]
    public class ProfileDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileDto"/> class.
        /// </summary>
        public ProfileDto()
        {
            Claims=new List<BusinessClaimDto>();
        }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        /// <value>
        /// The user's display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the login id.
		/// </summary>
		/// <value>
		/// The login id.
		/// </value>
		[DataMember]
		public string LoginId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

		/// <summary>
		/// Gets or sets the company id.
		/// </summary>
		/// <value>
		/// The company id.
		/// </value>
		[DataMember]
		public Guid CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        [DataMember]
        public IList<BusinessClaimDto> Claims { get; set; }

        /// <summary>
        /// Gets or sets the company external id.
        /// </summary>
        /// <value>
        /// The company external id.
        /// </value>
        [DataMember]
        public string CompanyExternalId { get; set; }

        /// <summary>
        /// Gets or sets the about me.
        /// </summary>
        /// <value>
        /// The about me.
        /// </value>
        [DataMember]
        public string AboutMe { get; set; }

		/// <summary>
		/// Gets or sets the name of the company.
		/// </summary>
		/// <value>
		/// The name of the company.
		/// </value>
		[DataMember]
		public string CompanyName { get; set; }

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