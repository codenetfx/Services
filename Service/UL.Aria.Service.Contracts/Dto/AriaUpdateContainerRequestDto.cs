using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Class AriaUpdateContainerRequestDto
    /// </summary>
    [DataContract]
    public class AriaUpdateContainerRequestDto
    {
        /// <summary>
        ///     Gets or sets the container id.
        /// </summary>
        /// <value>The container id.</value>
        [DataMember]
        public string ContainerId { get; set; }

        /// <summary>
        ///     Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [DataMember]
        public string MetaData { get; set; }
    }
}