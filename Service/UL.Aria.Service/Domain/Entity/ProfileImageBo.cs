namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Profile mage class.
    /// </summary>
    public class ProfileImageBo
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public byte[] Image { get; set; }


        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; set; }
    }
}