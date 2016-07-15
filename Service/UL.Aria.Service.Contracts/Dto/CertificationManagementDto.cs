using System;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Data Transfer Object for Certification Management.
    /// </summary>
    [DataContract]
    public class CertificationManagementDto
    {

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        [DataMember]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        [DataMember]
        public Guid TaskId { get; set; }

 
        /// <summary>
        /// Gets or sets the CCN industry.
        /// </summary>
        /// <value>
        /// The CCN industry.
        /// </value>
        [DataMember]
        public string CcnIndustry { get; set; }

        /// <summary>
        /// Gets or sets the CCN description.
        /// </summary>
        /// <value>
        /// The CCN description.
        /// </value>
        [DataMember]
        public string CcnDescription { get; set; }


        /// <summary>
        ///     Gets or sets the scope of this request.
        /// </summary>
        /// <value>
        ///     The scope of work.
        /// </value>
        [DataMember]
        public string ScopeOfRequest { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the department code.
        /// </summary>
        /// <value>
        /// The department code.
        /// </value>
        [DataMember]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is resubmittal.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is resubmittal; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsResubmittal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether work is from an outside lab (hence subject to Data Acceptance Program).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is outside lab; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsOutsideLab { get; set; }

        /// <summary>
        /// Gets or sets the standards and editions.
        /// </summary>
        /// <value>
        /// The standards and editions.
        /// </value>
        [DataMember]
        public string StandardsAndEditions { get; set; }

        /// <summary>
        /// Gets or sets the name of the submitting user.
        /// </summary>
        /// <value>
        /// The name of the submitting user.
        /// </value>
        [DataMember]
        public string SubmittingUserName { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }


        /// <summary>
        /// Gets or sets the handler location.
        /// </summary>
        /// <value>
        /// The handler location.
        /// </value>
        [DataMember]
        public string HandlerLocation { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        [DataMember]
        public DateTime CreatedDateTime { get; set; }
    }
}