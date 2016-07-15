using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Search criteria for fulfillment projects
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentProjectSearchCriteria
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [DataMember]
        public FulfillmentProjectSearchField FieldName { get; set; }

        /// <summary>
        /// Gets or sets the value for the field to be searched.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public string SearchValue { get; set; }
    }
}