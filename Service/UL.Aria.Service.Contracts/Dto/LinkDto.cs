using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class LinkDto.
	/// </summary>
    [DataContract]
	public class LinkDto : TrackedEntityDto
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkDto"/> class.
        /// </summary>
	    public LinkDto()
	    {
	        BusinessUnits = new List<BusinessUnitDto>();
	    }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the root URL.
        /// </summary>
        /// <value>
        /// The root URL.
        /// </value>
        [DataMember]
        public string RootUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the updated by login identifier.
        /// </summary>
        /// <value>
        /// The updated by login identifier.
        /// </value>
        [DataMember]
        public string UpdatedByLoginId { get; set; }
        /// <summary>
        /// Gets or sets the created by login identifier.
        /// </summary>
        /// <value>
        /// The created by login identifier.
        /// </value>
        [DataMember]        
        public string CreatedByLoginId { get; set; }


        /// <summary>
        /// Gets or sets the business units.
        /// </summary>
        /// <value>The business units.</value>
        [DataMember]
        public IEnumerable<BusinessUnitDto> BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the business unit codes.
        /// </summary>
        /// <value>The business unit codes.</value>
        [DataMember]
        public string BusinessUnitCodes { get; set; }

        /// <summary>
        /// Gets or sets the is modal.
        /// </summary>
        /// <value>
        /// The is modal.
        /// </value>
        [DataMember]
        public bool? IsModal { get; set; }
	}
}
