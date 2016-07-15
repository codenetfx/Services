using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Defines properties of a Certification Request.
    /// </summary>
    [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000001", Name = "CORequest")]
    public class CertificationRequestTaskProperty : TaskProperty
    {
        /// <summary>
        ///     The certification request task property type identifier
        /// </summary>
        public static readonly Guid CertificationRequestTaskPropertyTypeId =
            new Guid("00000000-0000-0000-0000-000000000001");

        /// <summary>
        ///     The certification request task property type name
        /// </summary>
        public const string CertificationRequestTaskPropertyTypeName = "CORequest";

        /// <summary>
        ///     Initializes a new instance of the <see cref="CertificationRequestTaskProperty" /> class.
        /// </summary>
        public CertificationRequestTaskProperty()
        {
            TaskPropertyTypeId = CertificationRequestTaskPropertyTypeId;
            TaskPropertyType = new TaskPropertyType(TaskPropertyTypeId)
            {
                Name = CertificationRequestTaskPropertyTypeName
            };
        }

        /// <summary>
        ///     Gets or sets the CCN.
        /// </summary>
        /// <value>
        ///     The CCN.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000002", Name = "CORequest.CCN")]
        public string CCN
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the comments to co.
        /// </summary>
        /// <value>
        ///     The comments to co.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000003", Name = "CORequest.Comments")]
        public string Comments
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the department code.
        /// </summary>
        /// <value>
        ///     The department code.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000004", Name = "CORequest.DepartmentCode")]
        public string DepartmentCode
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the file number.
        /// </summary>
        /// <value>
        ///     The file number.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000005", Name = "CORequest.FileNo")]
        public string FileNo
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the final name of the review.
        /// </summary>
        /// <value>
        ///     The final name of the review.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000006", Name = "CORequest.SubmittingUserName")]
        public string SubmittingUserName
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the project end date.
        /// </summary>
        /// <value>
        ///     The project end date.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000007", Name = "CORequest.ProjectEndDate")]
        public DateTime? ProjectEndDate
        {
            get
            {
                var childPropertyValue = GetChildPropertyValue();
                DateTime val;
                if (DateTime.TryParse(childPropertyValue, out val))
                    return val;
                return null;
            }
            set { SetChildPropertyValue(null == value ? null : value.ToString()); }
        }

        /// <summary>
        ///     Gets or sets the project handler.
        /// </summary>
        /// <value>
        ///     The project handler.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000008", Name = "CORequest.ProjectHandler")]
        public string ProjectHandler
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the project identifier.
        /// </summary>
        /// <value>
        ///     The project identifier.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000009", Name = "CORequest.ProjectId")]
        public Guid ProjectId
        {
            get
            {
                var childPropertyValue = GetChildPropertyValue();
                Guid val;
                if (Guid.TryParse(childPropertyValue, out val))
                    return val;
                return default(Guid);
            }
            set { SetChildPropertyValue(value.ToString()); }
        }

        /// <summary>
        ///     Gets or sets the scope of request.
        /// </summary>
        /// <value>
        ///     The scope of request.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000010", Name = "CORequest.ScopeOfRequest")]
        public string ScopeOfRequest
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the industry.
        /// </summary>
        /// <value>
        ///     The industry.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000011", Name = "CORequest.CcnIndustry")]
        public string CcnIndustry
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the CCN description.
        /// </summary>
        /// <value>
        ///     The CCN description.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000012", Name = "CORequest.CcnDescription")]
        public string CcnDescription
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the contact email.
        /// </summary>
        /// <value>
        ///     The contact email.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000013", Name = "CORequest.ContactEmail")]
        public string ContactEmail
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        ///     Gets or sets the name of the contact.
        /// </summary>
        /// <value>
        ///     The name of the contact.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000014", Name = "CORequest.ContactName")]
        public string ContactName
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the is outside lab.
        /// </summary>
        /// <value>
        /// The is outside lab.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000015", Name = "CORequest.IsOutsideLab")]
        public bool? IsOutsideLab
        {
            get
            {
                var childPropertyValue = GetChildPropertyValue();
                bool val;
                if (bool.TryParse(childPropertyValue, out val))
                    return val;
                return null;
            }
            set { SetChildPropertyValue(null == value ? null : value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the standards and editions.
        /// </summary>
        /// <value>
        /// The standards and editions.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000016", Name = "CORequest.StandardsAndEditions")]
        public string StandardsAndEditions
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the subscriber number.
        /// </summary>
        /// <value>
        /// The subscriber number.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000017", Name = "CORequest.SubscriberNumber")]
        public string SubscriberNumber
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the handler location.
        /// </summary>
        /// <value>
        /// The handler location.
        /// </value>
        [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000018", Name = "CORequest.HandlerLocation")]
        public string HandlerLocation
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }
        /// <summary>
        /// Gets or sets the project number.
        /// </summary>
        /// <value>
        /// The project number.
        /// </value>
         [TaskPropertyType(Id = "00000000-0000-0000-0000-000000000019", Name = "CORequest.ProjectNumber")]
        public string ProjectNumber
        {
            get { return GetChildPropertyValue(); }
            set { SetChildPropertyValue(value); }
        }
    }
}