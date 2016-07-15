using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Link classifier.
    /// </summary>
    public class Link: TrackedDomainEntity,IAuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        public Link()
        {
            BusinessUnits = new List<BusinessUnit>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Label { get; set; }


        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the root URL.
        /// </summary>
        /// <value>
        /// The root URL.
        /// </value>
        public string RootUrl { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return DisplayName; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return Label; }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate
        {
            get { return UpdatedDateTime; }
        }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IDictionary<string, string> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public string EntityType
        {
            get { return EntityTypeEnumDto.Link.ToString(); }
        }

        /// <summary>
        /// Gets or sets the business units.
        /// </summary>
        /// <value>The business units.</value>
        public IEnumerable<BusinessUnit> BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the business unit codes.
        /// </summary>
        /// <value>The business unit codes.</value>
        public string BusinessUnitCodes { get; set; }

        /// <summary>
        /// Gets or sets the updated by login identifier.
        /// </summary>
        /// <value>
        /// The updated by login identifier.
        /// </value>
        public string UpdatedByLoginId { get; set; }
        /// <summary>
        /// Gets or sets the created by login identifier.
        /// </summary>
        /// <value>
        /// The created by login identifier.
        /// </value>
        public string CreatedByLoginId { get; set; }

        /// <summary>
        /// Gets or sets the is modal.
        /// </summary>
        /// <value>
        /// The is modal.
        /// </value>
        public bool? IsModal { get; set; }
    }
}
