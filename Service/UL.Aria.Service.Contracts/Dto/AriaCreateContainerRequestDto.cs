using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class AriaCreateContainerRequestDto
    /// </summary>
    [DataContract]
    public class AriaCreateContainerRequestDto
    {
        /// <summary>
        ///     Gets or sets the type of the container.
        /// </summary>
        /// <value>The type of the container.</value>
        [DataMember]
        public string ContainerType { get; set; }

        /// <summary>
        ///     Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [DataMember]
        public string MetaData { get; set; }

        /// <summary>
        ///     Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        [DataMember]
        public string Container { get; set; }
    }
}