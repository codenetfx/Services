using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Search
{
	/// <summary>
	/// Represents a search result
	/// </summary>
	[Serializable]
    public class SearchResult:ISearchResult
	{
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid? Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the entity type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public EntityTypeEnumDto? EntityType { get; set; }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IDictionary<string, string> Metadata { get; set; }


        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        string ISearchResult.EntityType
        {
            get
            {
                if (this.EntityType == null)
                    return string.Empty;
                return this.EntityType.ToString();
            }           
        }
    }
}
