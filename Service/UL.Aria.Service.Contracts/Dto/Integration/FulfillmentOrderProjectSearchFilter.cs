using System.Collections.Generic;
using System.Runtime.Serialization;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// A single filter for a <see cref="FulfillmentOrderProjectSearchRequest"/>.
    /// One filter may have multiple values, when multiple values are used for same field, they are "OR"ed together.
    /// </summary>
    [DataContract]
    public class FulfillmentOrderProjectSearchFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FulfillmentOrderProjectSearchFilter"/> class.
        /// </summary>
        public FulfillmentOrderProjectSearchFilter()
        {
            SearchValues = new SearchValues();
        }
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [DataMember]
        public FulfillmentProjectSearchField FieldName { get; set; }

        /// <summary>
        /// Gets or sets the search values.
        /// </summary>
        /// <value>
        /// The search values.
        /// </value>
        [DataMember ()]
        public SearchValues SearchValues { get; set; }
    }

    /// <summary>
    /// A list of strings.
    /// </summary>
    [CollectionDataContract(ItemName = "SearchValue")]
    public class SearchValues : List<string>
    {
        
    }
}