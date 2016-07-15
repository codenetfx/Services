using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     Container search specification class.
    /// </summary>
     [KnownType(typeof(UL.Aria.Service.Domain.Search.Sort))]
    public class SearchCriteria:ISearchCriteria
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchCriteria" /> class.
        /// </summary>
        public SearchCriteria()
        {
            Refiners = new List<string>();
            Filters = new Dictionary<string, List<string>>();
            SearchCoordinators = new List<Provider.SearchCoordinator.ISearchCoordinator>();
            Sorts = new List<ISort>();
        }

        /// <summary>
        ///     Gets or sets the start index.
        /// </summary>
        /// <value>
        ///     The start index.
        /// </value>
        public long StartIndex { get; set; }

        /// <summary>
        ///     Gets or sets the end index.
        /// </summary>
        /// <value>
        ///     The end index.
        /// </value>
        public long EndIndex { get; set; }

        /// <summary>
        ///     Gets or sets the keyword.
        /// </summary>
        /// <value>
        ///     The keyword.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        ///     Gets or sets the sort by.
        /// </summary>
        /// <value>
        ///     The sort by.
        /// </value>
        public string SortBy { get; set; }

        /// <summary>
        ///     Gets or sets the sort direction.
        /// </summary>
        /// <value>
        ///     The sort direction.
        /// </value>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        ///     Gets or sets the user id.
        /// </summary>
        /// <value>
        ///     The user id.
        /// </value>
        public Guid? UserId { get; set; }

        /// <summary>
        ///     Gets or sets the company id.
        /// </summary>
        /// <value>
        ///     The company id.
        /// </value>
        public Guid? CompanyId { get; set; }

        /// <summary>
        ///     Gets or sets the asset type.
        /// </summary>
        /// <value>
        ///     The asset type.
        /// </value>
        public EntityTypeEnumDto? EntityType { get; set; }

        /// <summary>
        ///     Gets or sets the refiners.
        /// </summary>
        /// <value>The refiners.</value>
        public IList<string> Refiners { get; set; }

        /// <summary>
        ///     Gets or sets the filters.
        /// </summary>
        /// <value>
        ///     The filters.
        /// </value>
        public Dictionary<string, List<string>> Filters { get; set; }

        /// <summary>
        ///     Gets or sets the sorts.
        /// </summary>
        /// <value>The sorts.</value>
        public List<ISort> Sorts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include deleted records].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include deleted records]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeDeletedRecords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [filter containers].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [filter containers]; otherwise, <c>false</c>.
        /// </value>
        public bool FilterContainers { get; set; }

        /// <summary>
        /// Gets or sets the search coordinators that have been applied to this search criteria.
        /// </summary>
        /// <value>
        /// The search cooridators.
        /// </value>
        public List<Provider.SearchCoordinator.ISearchCoordinator> SearchCoordinators { get; set; }

		/// <summary>
		/// Gets or sets the additional filter string.
		/// </summary>
		/// <value>
		/// The additional filter string.
		/// </value>
	    public string AdditionalFilterString { get; set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        string ISearchCriteria.EntityTypeLabel
        {
            get
            {
                if (this.EntityType == null)
                    return string.Empty;
                return this.EntityType.ToString();
            }
            set
            {
                 EntityTypeEnumDto temp = EntityTypeEnumDto.Container;

                 if (Enum.TryParse<EntityTypeEnumDto>(value, out temp))
                 {
                     this.EntityType = temp;
                     return;
                 }                
            }
        }

      
    }
}