using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class SortDto
    /// </summary>
    [DataContract]
    public class SortDto
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string FieldName { get; set; }

        /// <summary>
        ///     Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        [DataMember]
        public SortDirectionDto Order { get; set; }
    }
}