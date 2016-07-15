using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;
namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Industry Code
    /// </summary>
    public abstract class LookupCodeBase : AuditableEntity, ISearchResult
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string ExternalId { get; set; }

        System.DateTime ISearchResult.ChangeDate
        {
            get { return this.UpdatedDateTime; }
        }

        string ISearchResult.EntityType
        {
            get { return string.Empty; }
        }

        /// <summary>
        ///Not implemented, returns null;
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        System.Collections.Generic.IDictionary<string, string> ISearchResult.Metadata
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string ISearchResult.Name
        {
            get { return this.Label; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string ISearchResult.Title
        {
            get { return this.Label; }
        }
    }
}