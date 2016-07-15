using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto.Integration
{
    /// <summary>
    /// Data Contract for holding a list of <see cref="FulfillmentProjectRefinerResult"/> objects.
    /// </summary>
    [CollectionDataContract(ItemName = "RefinerResult")]
    public class FulfillmentProjectRefinerResults : List<FulfillmentProjectRefinerResult>
    { }

    /// <summary>
    ///     Contract for refiners
    /// </summary>
    [DataContract(Namespace = "http://portal.ul.com")]
    public class FulfillmentProjectRefinerResultSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FulfillmentProjectRefinerResultSet"/> class.
        /// </summary>
        public FulfillmentProjectRefinerResultSet()
        {
            RefinerResults = new FulfillmentProjectRefinerResults();
        }
        /// <summary>
        ///     Gets or sets the refiner results.
        /// </summary>
        /// <value>
        ///     The refiner results.
        /// </value>
        [DataMember]
        public FulfillmentProjectRefinerResults RefinerResults { get; set; }

        /// <summary>
        ///     Gets or sets the refiner field.
        /// </summary>
        /// <value>
        ///     The refiner field.
        /// </value>
        [DataMember]
        public FulfillmentProjectRefinerField RefinerField { get; set; }
    }
}