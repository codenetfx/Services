using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    ///     Class Sort
    /// </summary>
    public class Sort:ISort
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sort"/> class.
        /// </summary>
        public Sort()
        {
            Order = SortDirection.Ascending;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string FieldName { get; set; }

        /// <summary>
        ///     Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public SortDirection Order { get; set; }
    }
}